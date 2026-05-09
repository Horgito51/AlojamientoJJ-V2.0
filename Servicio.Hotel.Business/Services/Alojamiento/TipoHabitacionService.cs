using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using Servicio.Hotel.Business.Mappers.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Interfaces;

namespace Servicio.Hotel.Business.Services.Alojamiento
{
    public class TipoHabitacionService : ITipoHabitacionService
    {
        private readonly ITipoHabitacionDataService _tipoHabitacionDataService;

        public TipoHabitacionService(ITipoHabitacionDataService tipoHabitacionDataService)
        {
            _tipoHabitacionDataService = tipoHabitacionDataService;
        }

        public async Task<TipoHabitacionDTO> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var dataModel = await _tipoHabitacionDataService.GetByIdAsync(id, ct);
            if (dataModel == null)
                throw new NotFoundException("TIP-001", $"No se encontró el tipo de habitación con ID {id}.");
            return dataModel.ToDto();
        }

        public async Task<TipoHabitacionDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var dataModel = await _tipoHabitacionDataService.GetByGuidAsync(guid, ct);
            if (dataModel == null)
                throw new NotFoundException("TIP-002", $"No se encontró el tipo de habitación con GUID {guid}.");
            return dataModel.ToDto();
        }

        public async Task<TipoHabitacionDTO> GetBySlugAsync(string slug, CancellationToken ct = default)
        {
            var dataModel = await _tipoHabitacionDataService.GetBySlugAsync(slug, ct);
            if (dataModel == null)
                throw new NotFoundException("TIP-002", $"No se encontró el tipo de habitación con slug '{slug}'.");
            return dataModel.ToDto();
        }

        public async Task<IEnumerable<TipoHabitacionDTO>> GetAllAsync(CancellationToken ct = default)
        {
            var pagedResult = await _tipoHabitacionDataService.GetAllPagedAsync(1, int.MaxValue, ct);
            return pagedResult.Items.ToDtoList();
        }

        public async Task<TipoHabitacionDTO> CreateAsync(TipoHabitacionCreateDTO tipoCreateDto, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(tipoCreateDto.CodigoTipoHabitacion))
                throw new ValidationException("TIP-003", "El código del tipo de habitación es obligatorio.");
            var baseSlug = SlugHelper.Slugify(tipoCreateDto.NombreTipoHabitacion);
            if (string.IsNullOrWhiteSpace(baseSlug))
                throw new ValidationException("TIP-003", "El nombre del tipo de habitación es obligatorio para generar el slug.");

            var slug = baseSlug;
            var suffix = 2;
            while (await _tipoHabitacionDataService.ExistsBySlugAsync(slug, ct))
            {
                slug = $"{baseSlug}-{suffix}";
                suffix++;
            }

            var dataModel = tipoCreateDto.ToDataModel();
            dataModel.Slug = slug;
            var created = await _tipoHabitacionDataService.AddAsync(dataModel, ct);
            return created.ToDto();
        }

        public async Task UpdateAsync(TipoHabitacionUpdateDTO tipoUpdateDto, CancellationToken ct = default)
        {
            var existing = await _tipoHabitacionDataService.GetByIdAsync(tipoUpdateDto.IdTipoHabitacion, ct);
            if (existing == null)
                throw new NotFoundException("TIP-004", $"No se encontró el tipo de habitación con ID {tipoUpdateDto.IdTipoHabitacion}.");
            var dataModel = tipoUpdateDto.ToDataModel();
            await _tipoHabitacionDataService.UpdateAsync(dataModel, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _tipoHabitacionDataService.GetByIdAsync(id, ct);
            if (existing == null)
                throw new NotFoundException("TIP-005", $"No se encontró el tipo de habitación con ID {id}.");
            await _tipoHabitacionDataService.DeleteAsync(id, ct);
        }

        public async Task<IEnumerable<TipoHabitacionDTO>> GetPublicosAsync(CancellationToken ct = default)
        {
            var dataModels = await _tipoHabitacionDataService.GetPublicosAsync(ct);
            return dataModels.ToDtoList();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken ct = default)
        {
            return await _tipoHabitacionDataService.ExistsByCodigoAsync(codigo, ct);
        }
    }
}
