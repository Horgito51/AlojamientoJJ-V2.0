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
    public class HabitacionDataService : IHabitacionDataService
    {
        private readonly IHabitacionRepository _habitacionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HabitacionDataService(IHabitacionRepository habitacionRepository, IUnitOfWork unitOfWork)
        {
            _habitacionRepository = habitacionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<HabitacionDataModel> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _habitacionRepository.GetByIdAsync(id, ct);
            return entity?.ToModel();
        }

        public async Task<HabitacionDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var entity = await _habitacionRepository.GetByGuidAsync(guid, ct);
            return entity?.ToModel();
        }

        public async Task<DataPagedResult<HabitacionDataModel>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var entities = await _habitacionRepository.GetAllAsync(ct);
            var items = entities.ToModelList();
            var totalCount = items.Count;
            var pagedItems = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new DataPagedResult<HabitacionDataModel>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<HabitacionDataModel> AddAsync(HabitacionDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            if (entity.HabitacionGuid == Guid.Empty) entity.HabitacionGuid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.CreadoPorUsuario)) entity.CreadoPorUsuario = "Sistema";
            if (string.IsNullOrWhiteSpace(entity.ServicioOrigen)) entity.ServicioOrigen = "habitaciones-service";
            entity.FechaRegistroUtc = DateTime.UtcNow;
            var added = await _habitacionRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return added.ToModel();
        }

        public async Task UpdateAsync(HabitacionDataModel model, CancellationToken ct = default)
        {
            var entity = await _habitacionRepository.GetByIdAsync(model.IdHabitacion, ct);
            if (entity == null) return;

            entity.IdTipoHabitacion = model.IdTipoHabitacion;
            entity.NumeroHabitacion = model.NumeroHabitacion;
            entity.Piso = model.Piso;
            entity.CapacidadHabitacion = model.CapacidadHabitacion;
            entity.PrecioBase = model.PrecioBase;
            entity.Url = model.Url;
            entity.DescripcionHabitacion = model.DescripcionHabitacion;
            entity.EstadoHabitacion = model.EstadoHabitacion;
            entity.ModificadoPorUsuario = model.ModificadoPorUsuario ?? "Sistema";
            entity.FechaModificacionUtc = DateTime.UtcNow;

            await _habitacionRepository.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _habitacionRepository.DeleteAsync(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<DataPagedResult<HabitacionDataModel>> GetByFiltroAsync(HabitacionFiltroDataModel filtro, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var all = await _habitacionRepository.GetAllAsync(ct);
            var query = all.AsQueryable();

            if (filtro.IdSucursal.HasValue)
                query = query.Where(h => h.IdSucursal == filtro.IdSucursal.Value);
            if (filtro.IdTipoHabitacion.HasValue)
                query = query.Where(h => h.IdTipoHabitacion == filtro.IdTipoHabitacion.Value);
            if (!string.IsNullOrEmpty(filtro.EstadoHabitacion))
                query = query.Where(h => h.EstadoHabitacion == filtro.EstadoHabitacion);
            if (!string.IsNullOrEmpty(filtro.NumeroHabitacion))
                query = query.Where(h => h.NumeroHabitacion.Contains(filtro.NumeroHabitacion));
            if (filtro.EsEliminado.HasValue)
                query = query.Where(h => h.EsEliminado == filtro.EsEliminado.Value);

            var totalCount = query.Count();
            var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new DataPagedResult<HabitacionDataModel>
            {
                Items = items.ToModelList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<HabitacionDataModel>> GetBySucursalAsync(int idSucursal, CancellationToken ct = default)
        {
            var entities = await _habitacionRepository.GetBySucursalAsync(idSucursal, ct);
            return entities.ToModelList();
        }

        public async Task<IEnumerable<HabitacionDataModel>> GetByTipoHabitacionAsync(int idTipoHabitacion, CancellationToken ct = default)
        {
            var entities = await _habitacionRepository.GetByTipoHabitacionAsync(idTipoHabitacion, ct);
            return entities.ToModelList();
        }

        public async Task UpdateEstadoAsync(int id, string nuevoEstado, string usuario, CancellationToken ct = default)
        {
            await _habitacionRepository.UpdateEstadoAsync(id, nuevoEstado, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<HabitacionDataModel>> GetDisponiblesAsync(int idSucursal, DateTime inicio, DateTime fin, CancellationToken ct = default)
        {
            var entities = await _habitacionRepository.GetDisponiblesAsync(idSucursal, inicio, fin, ct);
            return entities.ToModelList();
        }
    }
}
