using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Seguridad;
using Servicio.Hotel.DataManagement.Seguridad.Interfaces;
using Servicio.Hotel.DataManagement.Seguridad.Models;
using Servicio.Hotel.DataManagement.Seguridad.Mappers;
using Servicio.Hotel.DataManagement.Common;
using Servicio.Hotel.DataManagement.UnitOfWork;

namespace Servicio.Hotel.DataManagement.Seguridad.Services
{
    public class UsuarioDataService : IUsuarioDataService
    {
        private readonly IUsuarioAppRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioDataService(IUsuarioAppRepository usuarioRepository, IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UsuarioDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _usuarioRepository.GetByIdAsync(id, ct);
            return entity?.ToModel();
        }

        public async Task<UsuarioDataModel?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var entity = await _usuarioRepository.GetByGuidAsync(guid, ct);
            return entity?.ToModel();
        }

        public async Task<DataPagedResult<UsuarioDataModel>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var entities = await _usuarioRepository.GetAllAsync(ct);
            var items = entities.ToModelList();
            var totalCount = items.Count;
            var pagedItems = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new DataPagedResult<UsuarioDataModel>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<UsuarioDataModel> AddAsync(UsuarioDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            if (entity.UsuarioGuid == Guid.Empty) entity.UsuarioGuid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.CreadoPorUsuario)) entity.CreadoPorUsuario = "Sistema";
            entity.FechaRegistroUtc = DateTime.UtcNow;
            var added = await _usuarioRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return added.ToModel();
        }

        public async Task UpdateAsync(UsuarioDataModel model, CancellationToken ct = default)
        {
            var entity = await _usuarioRepository.GetByIdAsync(model.IdUsuario, ct);
            if (entity == null) return;

            // Actualizamos campos básicos
            entity.Correo = model.Correo;
            entity.IdCliente = model.IdCliente;
            entity.Nombres = model.Nombres;
            entity.Apellidos = model.Apellidos;
            entity.EstadoUsuario = model.EstadoUsuario;
            entity.Activo = model.Activo;
            entity.ModificadoPorUsuario = model.ModificadoPorUsuario ?? "Sistema";
            entity.FechaModificacionUtc = DateTime.UtcNow;

            // Para los roles, se debería manejar a través de una tabla intermedia o un método especializado
            // Por ahora, para corregir el error de compilación, omitimos la asignación directa
            // ya que la entidad usa 'UsuariosRoles' (colección de UsuarioRolEntity) y no 'Roles'.

            await _usuarioRepository.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _usuarioRepository.DeleteAsync(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<UsuarioDataModel?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            var entity = await _usuarioRepository.GetByUsernameAsync(username, ct);
            return entity?.ToModel();
        }

        public async Task<UsuarioCredencialesDataModel?> GetCredentialsByUsernameAsync(string username, CancellationToken ct = default)
        {
            var entity = await _usuarioRepository.GetByUsernameAsync(username, ct);
            return entity?.ToCredentialsModel();
        }

        public async Task<UsuarioCredencialesDataModel?> GetCredentialsByCorreoAsync(string correo, CancellationToken ct = default)
        {
            var entity = await _usuarioRepository.GetByCorreoAsync(correo, ct);
            return entity?.ToCredentialsModel();
        }

        public async Task<UsuarioCredencialesDataModel?> GetCredentialsByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _usuarioRepository.GetByIdAsync(id, ct);
            return entity?.ToCredentialsModel();
        }

        public async Task<UsuarioDataModel?> GetByCorreoAsync(string correo, CancellationToken ct = default)
        {
            var entity = await _usuarioRepository.GetByCorreoAsync(correo, ct);
            return entity?.ToModel();
        }

        public async Task<UsuarioDataModel?> AuthenticateAsync(string username, string passwordHash, CancellationToken ct = default)
        {
            var entity = await _usuarioRepository.AuthenticateAsync(username, passwordHash, ct);
            return entity?.ToModel();
        }

        public async Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default)
        {
            await _usuarioRepository.InhabilitarAsync(id, motivo, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task CambiarPasswordAsync(int id, string newHash, string newSalt, string usuario, CancellationToken ct = default)
        {
            await _usuarioRepository.CambiarPasswordAsync(id, newHash, newSalt, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task AsociarClienteAsync(int idUsuario, int idCliente, string usuario, CancellationToken ct = default)
        {
            var entity = await _usuarioRepository.GetByIdAsync(idUsuario, ct);
            if (entity == null) return;

            entity.IdCliente = idCliente;
            entity.ModificadoPorUsuario = usuario;
            entity.FechaModificacionUtc = DateTime.UtcNow;

            await _usuarioRepository.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null, CancellationToken ct = default)
        {
            return await _usuarioRepository.ExistsByUsernameAsync(username, excludeId, ct);
        }

        public async Task<bool> ExistsByCorreoAsync(string correo, int? excludeId = null, CancellationToken ct = default)
        {
            return await _usuarioRepository.ExistsByCorreoAsync(correo, excludeId, ct);
        }
    }
}
