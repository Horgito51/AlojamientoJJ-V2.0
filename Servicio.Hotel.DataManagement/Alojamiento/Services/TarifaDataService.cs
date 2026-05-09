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
    public class TarifaDataService : ITarifaDataService
    {
        private readonly ITarifaRepository _tarifaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TarifaDataService(ITarifaRepository tarifaRepository, IUnitOfWork unitOfWork)
        {
            _tarifaRepository = tarifaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TarifaDataModel> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _tarifaRepository.GetByIdAsync(id, ct);
            return entity?.ToModel();
        }

        public async Task<TarifaDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var entity = await _tarifaRepository.GetByGuidAsync(guid, ct);
            return entity?.ToModel();
        }

        public async Task<DataPagedResult<TarifaDataModel>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var entities = await _tarifaRepository.GetAllAsync(ct);
            var items = entities.ToModelList();
            var totalCount = items.Count;
            var pagedItems = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new DataPagedResult<TarifaDataModel>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<TarifaDataModel> AddAsync(TarifaDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            if (entity.TarifaGuid == Guid.Empty) entity.TarifaGuid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.CreadoPorUsuario)) entity.CreadoPorUsuario = "Sistema";
            if (string.IsNullOrWhiteSpace(entity.ServicioOrigen)) entity.ServicioOrigen = "tarifas-service";
            entity.FechaRegistroUtc = DateTime.UtcNow;
            var added = await _tarifaRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return added.ToModel();
        }

        public async Task UpdateAsync(TarifaDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            await _tarifaRepository.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _tarifaRepository.DeleteAsync(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<TarifaDataModel> GetTarifaVigenteAsync(int idSucursal, int idTipoHabitacion, DateTime fecha, string? canal = null, CancellationToken ct = default)
        {
            var entity = await _tarifaRepository.GetTarifaVigenteAsync(idSucursal, idTipoHabitacion, fecha, canal, ct);
            return entity?.ToModel();
        }

        public async Task<TarifaDataModel> GetTarifaVigenteRangoAsync(int idSucursal, int idTipoHabitacion, DateTime fechaInicio, DateTime fechaFin, string? canal = null, CancellationToken ct = default)
        {
            var entity = await _tarifaRepository.GetTarifaVigenteRangoAsync(idSucursal, idTipoHabitacion, fechaInicio, fechaFin, canal, ct);
            return entity?.ToModel();
        }

        public async Task<IEnumerable<TarifaDataModel>> GetBySucursalAsync(int idSucursal, CancellationToken ct = default)
        {
            var entities = await _tarifaRepository.GetBySucursalAsync(idSucursal, ct);
            return entities.ToModelList();
        }

        public async Task DesactivarAsync(int id, string usuario, CancellationToken ct = default)
        {
            await _tarifaRepository.DesactivarAsync(id, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
