using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Seguridad;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Seguridad;
using Servicio.Hotel.Business.Mappers.Seguridad;
using Servicio.Hotel.Business.Validators.Seguridad; // Podrías crear un UsuarioValidator
using Servicio.Hotel.DataManagement.Seguridad.Interfaces;

namespace Servicio.Hotel.Business.Services.Seguridad
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioDataService _usuarioDataService;
        private readonly IRolDataService _rolDataService;

        public UsuarioService(IUsuarioDataService usuarioDataService, IRolDataService rolDataService)
        {
            _usuarioDataService = usuarioDataService;
            _rolDataService = rolDataService;
        }

        public async Task<UsuarioDTO> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var dataModel = await _usuarioDataService.GetByIdAsync(id, ct);
            if (dataModel == null)
                throw new NotFoundException("USR-001", $"No se encontró el usuario con ID {id}.");
            return dataModel.ToDto();
        }

        public async Task<UsuarioDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var dataModel = await _usuarioDataService.GetByGuidAsync(guid, ct);
            if (dataModel == null)
                throw new NotFoundException("USR-002", $"No se encontró el usuario con GUID {guid}.");
            return dataModel.ToDto();
        }

        public async Task<PagedResult<UsuarioDTO>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var pagedData = await _usuarioDataService.GetAllPagedAsync(pageNumber, pageSize, ct);
            return new PagedResult<UsuarioDTO>
            {
                Items = pagedData.Items.ToDtoList(),
                TotalCount = pagedData.TotalCount,
                PageNumber = pagedData.PageNumber,
                PageSize = pagedData.PageSize
            };
        }

        public async Task<UsuarioDTO> CreateAsync(UsuarioCreateDTO usuarioCreateDto, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(usuarioCreateDto.Username))
                throw new ValidationException("USR-003", "El nombre de usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(usuarioCreateDto.Correo))
                throw new ValidationException("USR-004", "El correo es obligatorio.");
            if (string.IsNullOrWhiteSpace(usuarioCreateDto.Password) || usuarioCreateDto.Password.Length < 10)
                throw new ValidationException("USR-013", "La contraseña es obligatoria y debe tener al menos 10 caracteres.");
            if (string.IsNullOrWhiteSpace(usuarioCreateDto.Nombres))
                throw new ValidationException("USR-012", "Los nombres son obligatorios.");

            // Contrato actual: lista de IDs de roles (Roles: [1,2])
            var roleIds = usuarioCreateDto.Roles?
                .Select(r => r.IdRol)
                .Where(id => id > 0)
                .Distinct()
                .ToList() ?? new();

            if (roleIds.Count > 0)
            {
                var resolved = new System.Collections.Generic.List<RolDTO>();
                foreach (var roleId in roleIds)
                {
                    var rol = await _rolDataService.GetByIdAsync(roleId, ct);
                    if (rol == null)
                        throw new NotFoundException("ROL-001", $"Rol no encontrado con ID {roleId}.");

                    if (rol.EsEliminado || rol.EstadoRol == "INA")
                        throw new BusinessException("El rol especificado no esta activo.");

                    resolved.Add(rol.ToDto());
                }

                usuarioCreateDto.IdRol = null;
                usuarioCreateDto.RolGuid = null;
                usuarioCreateDto.Roles = resolved;
            }

if (usuarioCreateDto.IdRol.HasValue && (usuarioCreateDto.Roles == null || usuarioCreateDto.Roles.Count == 0))
            {
                var rol = await _rolDataService.GetByIdAsync(usuarioCreateDto.IdRol.Value, ct);
                if (rol == null)
                    throw new NotFoundException("Rol no encontrado");

                if (rol.EsEliminado || rol.EstadoRol == "INA")
                    throw new BusinessException("El rol especificado no está activo");

                usuarioCreateDto.Roles = new() { rol.ToDto() };
            }
            else if (usuarioCreateDto.RolGuid.HasValue)
            {
                // Compatibilidad: algunos clientes antiguos envían rol por GUID
                var rol = await _rolDataService.GetByGuidAsync(usuarioCreateDto.RolGuid.Value, ct);
                if (rol == null)
                    throw new NotFoundException("Rol no encontrado");

                if (rol.EsEliminado || rol.EstadoRol == "INA")
                    throw new BusinessException("El rol especificado no está activo");

                usuarioCreateDto.Roles = new() { rol.ToDto() };
            }

            if (usuarioCreateDto.Roles == null || usuarioCreateDto.Roles.Count == 0)
                throw new ValidationException("USR-011", "El usuario debe tener al menos un rol.");

            var (hash, salt) = PasswordHasher.HashPassword(usuarioCreateDto.Password);

            var dataModel = new Servicio.Hotel.DataManagement.Seguridad.Models.UsuarioDataModel
            {
                IdCliente = usuarioCreateDto.IdCliente,
                Username = usuarioCreateDto.Username,
                Correo = usuarioCreateDto.Correo,
                Nombres = usuarioCreateDto.Nombres,
                Apellidos = usuarioCreateDto.Apellidos,
                EstadoUsuario = usuarioCreateDto.EstadoUsuario,
                Activo = usuarioCreateDto.Activo,
                PasswordHash = hash,
                PasswordSalt = salt,
                Roles = usuarioCreateDto.Roles?.Select(r => r.ToDataModel()).ToList()
            };

            var created = await _usuarioDataService.AddAsync(dataModel, ct);
            return created.ToDto();
        }

        public async Task UpdateAsync(UsuarioUpdateDTO usuarioUpdateDto, CancellationToken ct = default)
        {
            var existing = await _usuarioDataService.GetByIdAsync(usuarioUpdateDto.IdUsuario, ct);
            if (existing == null)
                throw new NotFoundException("USR-005", $"No se encontró el usuario con ID {usuarioUpdateDto.IdUsuario}.");

            // Solo actualizamos los campos permitidos en la actualización
            existing.Correo = usuarioUpdateDto.Correo;
            existing.Nombres = usuarioUpdateDto.Nombres;
            existing.Apellidos = usuarioUpdateDto.Apellidos;
            existing.EstadoUsuario = usuarioUpdateDto.EstadoUsuario;
            existing.Activo = usuarioUpdateDto.Activo;
            existing.Roles = usuarioUpdateDto.Roles?.Select(r => r.ToDataModel()).ToList();

            await _usuarioDataService.UpdateAsync(existing, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _usuarioDataService.GetByIdAsync(id, ct);
            if (existing == null)
                throw new NotFoundException("USR-006", $"No se encontró el usuario con ID {id}.");
            await _usuarioDataService.DeleteAsync(id, ct);
        }

        public async Task<UsuarioDTO> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            var dataModel = await _usuarioDataService.GetByUsernameAsync(username, ct);
            if (dataModel == null)
                throw new NotFoundException("USR-007", $"No se encontró usuario con nombre {username}.");
            return dataModel.ToDto();
        }

        public async Task<UsuarioDTO> GetByCorreoAsync(string correo, CancellationToken ct = default)
        {
            var dataModel = await _usuarioDataService.GetByCorreoAsync(correo, ct);
            if (dataModel == null)
                throw new NotFoundException("USR-008", $"No se encontró usuario con correo {correo}.");
            return dataModel.ToDto();
        }

        public async Task AsociarClienteAsync(int idUsuario, int idCliente, string usuario, CancellationToken ct = default)
        {
            var existing = await _usuarioDataService.GetByIdAsync(idUsuario, ct);
            if (existing == null)
                throw new NotFoundException("USR-015", $"No se encontró el usuario con ID {idUsuario}.");

            await _usuarioDataService.AsociarClienteAsync(idUsuario, idCliente, usuario, ct);
        }

        public async Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default)
        {
            var existing = await _usuarioDataService.GetByIdAsync(id, ct);
            if (existing == null)
                throw new NotFoundException("USR-009", $"No se encontró el usuario con ID {id}.");
            await _usuarioDataService.InhabilitarAsync(id, motivo, usuario, ct);
        }

        public async Task CambiarPasswordAsync(int id, string newPassword, string usuario, CancellationToken ct = default)
        {
            var existing = await _usuarioDataService.GetByIdAsync(id, ct);
            if (existing == null)
                throw new NotFoundException("USR-010", $"No se encontró el usuario con ID {id}.");
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 10)
                throw new ValidationException("USR-014", "La nueva contraseña debe tener al menos 10 caracteres.");

            var (hash, salt) = PasswordHasher.HashPassword(newPassword);
            await _usuarioDataService.CambiarPasswordAsync(id, hash, salt, usuario, ct);
        }

        public async Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null, CancellationToken ct = default)
        {
            return await _usuarioDataService.ExistsByUsernameAsync(username, excludeId, ct);
        }

        public async Task<bool> ExistsByCorreoAsync(string correo, int? excludeId = null, CancellationToken ct = default)
        {
            return await _usuarioDataService.ExistsByCorreoAsync(correo, excludeId, ct);
        }
    }
}
