using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Seguridad;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Seguridad;
using Servicio.Hotel.Business.Mappers.Seguridad;
using Servicio.Hotel.DataManagement.Seguridad.Interfaces;

namespace Servicio.Hotel.Business.Services.Seguridad
{
    public class RolService : IRolService
    {
        private readonly IRolDataService _rolDataService;

        public RolService(IRolDataService rolDataService)
        {
            _rolDataService = rolDataService;
        }

        public async Task<RolDTO> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var dataModel = await _rolDataService.GetByIdAsync(id, ct);
            if (dataModel == null)
                throw new NotFoundException("ROL-001", $"No se encontró el rol con ID {id}.");
            return dataModel.ToDto();
        }

        public async Task<RolDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var dataModel = await _rolDataService.GetByGuidAsync(guid, ct);
            if (dataModel == null)
                throw new NotFoundException("ROL-002", $"No se encontró el rol con GUID {guid}.");
            return dataModel.ToDto();
        }

        public async Task<PagedResult<RolDTO>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var pagedData = await _rolDataService.GetAllPagedAsync(pageNumber, pageSize, ct);
            return new PagedResult<RolDTO>
            {
                Items = pagedData.Items.ToDtoList(),
                TotalCount = pagedData.TotalCount,
                PageNumber = pagedData.PageNumber,
                PageSize = pagedData.PageSize
            };
        }

        public async Task<RolDTO> CreateAsync(RolCreateDTO rolCreateDto, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(rolCreateDto.NombreRol))
                throw new ValidationException("ROL-003", "El nombre del rol es obligatorio.");
            var dataModel = new Servicio.Hotel.DataManagement.Seguridad.Models.RolDataModel
            {
                NombreRol = rolCreateDto.NombreRol,
                DescripcionRol = rolCreateDto.DescripcionRol,
                EstadoRol = rolCreateDto.EstadoRol,
                Activo = rolCreateDto.Activo
            };
            var created = await _rolDataService.AddAsync(dataModel, ct);
            return created.ToDto();
        }

        public async Task UpdateAsync(RolUpdateDTO rolUpdateDto, CancellationToken ct = default)
        {
            var existing = await _rolDataService.GetByIdAsync(rolUpdateDto.IdRol, ct);
            if (existing == null)
                throw new NotFoundException("ROL-004", $"No se encontró el rol con ID {rolUpdateDto.IdRol}.");
            existing.NombreRol = rolUpdateDto.NombreRol;
            existing.DescripcionRol = rolUpdateDto.DescripcionRol;
            existing.EstadoRol = rolUpdateDto.EstadoRol;
            existing.Activo = rolUpdateDto.Activo;
            await _rolDataService.UpdateAsync(existing, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _rolDataService.GetByIdAsync(id, ct);
            if (existing == null)
                throw new NotFoundException("ROL-005", $"No se encontró el rol con ID {id}.");
            await _rolDataService.DeleteAsync(id, ct);
        }

        public async Task<RolDTO> GetByNombreAsync(string nombre, CancellationToken ct = default)
        {
            var dataModel = await _rolDataService.GetByNombreAsync(nombre, ct);
            if (dataModel == null)
                throw new NotFoundException("ROL-006", $"No se encontró el rol con nombre {nombre}.");
            return dataModel.ToDto();
        }

        public async Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default)
        {
            var existing = await _rolDataService.GetByIdAsync(id, ct);
            if (existing == null)
                throw new NotFoundException("ROL-007", $"No se encontró el rol con ID {id}.");
            await _rolDataService.InhabilitarAsync(id, motivo, usuario, ct);
        }

        public async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null, CancellationToken ct = default)
        {
            return await _rolDataService.ExistsByNombreAsync(nombre, excludeId, ct);
        }
    }
}