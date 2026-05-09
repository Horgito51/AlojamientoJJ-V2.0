using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using Servicio.Hotel.Business.Mappers.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Interfaces;

namespace Servicio.Hotel.Business.Services.Alojamiento
{
    public class CatalogoServicioService : ICatalogoServicioService
    {
        private readonly ICatalogoServicioDataService _dataService;

        public CatalogoServicioService(ICatalogoServicioDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IEnumerable<CatalogoServicioDTO>> GetAllAsync(CancellationToken ct = default)
            => (await _dataService.GetAllAsync(ct)).ToDtoList();

        public async Task<CatalogoServicioDTO> GetByIdAsync(int id, CancellationToken ct = default)
            => (await _dataService.GetByIdAsync(id, ct)).ToDto()
               ?? throw new NotFoundException("CAT-001", $"No se encontró el catálogo de servicio con ID {id}.");

        public async Task<CatalogoServicioDTO> CreateAsync(CatalogoServicioCreateDTO dto, CancellationToken ct = default)
        {
            var tiposValidos = new[] { "AME", "SRV" };
            if (string.IsNullOrWhiteSpace(dto.TipoCatalogo) || !tiposValidos.Contains(dto.TipoCatalogo.ToUpper()))
                throw new ValidationException("CAT-002", "El tipo de catálogo debe ser 'AME' (amenidad) o 'SRV' (servicio).");

            dto.TipoCatalogo = dto.TipoCatalogo.ToUpper();

            var created = await _dataService.AddAsync(dto.ToDataModel()!, ct);
            return created.ToDto()!;
        }

public async Task UpdateAsync(CatalogoServicioUpdateDTO dto, CancellationToken ct = default)
        {
            var existing = await _dataService.GetByIdAsync(dto.IdCatalogo, ct);
            if (existing == null)
                throw new NotFoundException("CAT-001", $"No se encontró el catálogo de servicio con ID {dto.IdCatalogo}.");

            var tiposValidos = new[] { "AME", "SRV" };
            if (!string.IsNullOrWhiteSpace(dto.TipoCatalogo) && !tiposValidos.Contains(dto.TipoCatalogo.ToUpper()))
                throw new ValidationException("CAT-003", "El tipo de catálogo debe ser 'AME' (amenidad) o 'SRV' (servicio).");

            if (!string.IsNullOrWhiteSpace(dto.TipoCatalogo))
                dto.TipoCatalogo = dto.TipoCatalogo.ToUpper();

            if (existing.EsEliminado)
                throw new ConflictException("No se puede actualizar un item eliminado logicamente.");

            existing.IdSucursal = dto.IdSucursal ?? existing.IdSucursal;
            existing.CodigoCatalogo = dto.CodigoCatalogo;
            existing.NombreCatalogo = dto.NombreCatalogo;
            existing.TipoCatalogo = dto.TipoCatalogo;
            existing.CategoriaCatalogo = dto.CategoriaCatalogo ?? string.Empty;
            existing.DescripcionCatalogo = dto.DescripcionCatalogo ?? string.Empty;
            existing.PrecioBase = dto.PrecioBase;
            existing.AplicaIva = dto.AplicaIva;
            existing.Disponible24h = dto.Disponible24h;
            existing.HoraInicio = dto.HoraInicio;
            existing.HoraFin = dto.HoraFin;
            existing.IconoUrl = dto.IconoUrl ?? string.Empty;
            existing.EstadoCatalogo = dto.EstadoCatalogo;
            existing.ModificadoPorUsuario = "Sistema";
            existing.FechaModificacionUtc = DateTime.UtcNow;

            await _dataService.UpdateAsync(existing, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            _ = await GetByIdAsync(id, ct);
            await _dataService.DeleteAsync(id, ct);
        }
    }
}
