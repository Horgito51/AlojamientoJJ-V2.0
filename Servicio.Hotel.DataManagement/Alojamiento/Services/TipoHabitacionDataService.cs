using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Interfaces;
using Servicio.Hotel.DataManagement.Alojamiento.Models;
using Servicio.Hotel.DataManagement.Alojamiento.Mappers;
using Servicio.Hotel.DataManagement.Common;
using Servicio.Hotel.DataManagement.UnitOfWork;

namespace Servicio.Hotel.DataManagement.Alojamiento.Services
{
    public class TipoHabitacionDataService : ITipoHabitacionDataService
    {
        private readonly ITipoHabitacionRepository _tipoHabitacionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TipoHabitacionDataService(ITipoHabitacionRepository tipoHabitacionRepository, IUnitOfWork unitOfWork)
        {
            _tipoHabitacionRepository = tipoHabitacionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TipoHabitacionDataModel> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _tipoHabitacionRepository.GetByIdAsync(id, ct);
            return entity?.ToModel();
        }

        public async Task<TipoHabitacionDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var entity = await _tipoHabitacionRepository.GetByGuidAsync(guid, ct);
            return entity?.ToModel();
        }

        public async Task<TipoHabitacionDataModel> GetBySlugAsync(string slug, CancellationToken ct = default)
        {
            var entity = await _tipoHabitacionRepository.GetBySlugAsync(slug, ct);
            return entity?.ToModel();
        }

        public async Task<DataPagedResult<TipoHabitacionDataModel>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var entities = await _tipoHabitacionRepository.GetAllAsync(ct);
            var items = entities.ToModelList();
            var totalCount = items.Count;
            var pagedItems = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new DataPagedResult<TipoHabitacionDataModel>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<TipoHabitacionDataModel> AddAsync(TipoHabitacionDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            if (entity.TipoHabitacionGuid == Guid.Empty) entity.TipoHabitacionGuid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.CreadoPorUsuario)) entity.CreadoPorUsuario = "Sistema";
            if (string.IsNullOrWhiteSpace(entity.ServicioOrigen)) entity.ServicioOrigen = "habitaciones-service";
            entity.FechaRegistroUtc = DateTime.UtcNow;
            var added = await _tipoHabitacionRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return added.ToModel();
        }

        public async Task UpdateAsync(TipoHabitacionDataModel model, CancellationToken ct = default)
        {
            var existing = await _tipoHabitacionRepository.GetByIdAsync(model.IdTipoHabitacion, ct);
            if (existing == null) return;

            existing.CodigoTipoHabitacion = model.CodigoTipoHabitacion;
            existing.NombreTipoHabitacion = model.NombreTipoHabitacion;
            existing.Descripcion = model.Descripcion;
            existing.CapacidadAdultos = model.CapacidadAdultos;
            existing.CapacidadNinos = model.CapacidadNinos;
            existing.CapacidadTotal = model.CapacidadTotal;
            existing.TipoCama = model.TipoCama;
            existing.AreaM2 = model.AreaM2;
            existing.PermiteEventos = model.PermiteEventos;
            existing.PermiteReservaPublica = model.PermiteReservaPublica;
            existing.EstadoTipoHabitacion = model.EstadoTipoHabitacion;
            existing.ModificadoPorUsuario = string.IsNullOrWhiteSpace(model.ModificadoPorUsuario) ? "Sistema" : model.ModificadoPorUsuario;
            existing.FechaModificacionUtc = DateTime.UtcNow;

            await _tipoHabitacionRepository.UpdateAsync(existing, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _tipoHabitacionRepository.DeleteAsync(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<TipoHabitacionDataModel>> GetPublicosAsync(CancellationToken ct = default)
        {
            var entities = await _tipoHabitacionRepository.GetPublicosAsync(ct);
            return entities.ToModelList();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken ct = default)
        {
            return await _tipoHabitacionRepository.ExistsByCodigoAsync(codigo, ct);
        }

        public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken ct = default)
        {
            return await _tipoHabitacionRepository.ExistsBySlugAsync(slug, ct);
        }
    }
}
