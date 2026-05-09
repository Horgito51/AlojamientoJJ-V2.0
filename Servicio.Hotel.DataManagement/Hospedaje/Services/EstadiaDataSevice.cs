using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Hospedaje;
using Servicio.Hotel.DataManagement.Hospedaje.Interfaces;
using Servicio.Hotel.DataManagement.Hospedaje.Models;
using Servicio.Hotel.DataManagement.Hospedaje.Mappers;
using Servicio.Hotel.DataManagement.Common;
using Servicio.Hotel.DataManagement.UnitOfWork;

namespace Servicio.Hotel.DataManagement.Hospedaje.Services
{
    public class EstadiaDataService : IEstadiaDataService
    {
        private readonly IEstadiaRepository _estadiaRepository;
        private readonly ICargoEstadiaRepository _cargoEstadiaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EstadiaDataService(IEstadiaRepository estadiaRepository, ICargoEstadiaRepository cargoEstadiaRepository, IUnitOfWork unitOfWork)
        {
            _estadiaRepository = estadiaRepository;
            _cargoEstadiaRepository = cargoEstadiaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<EstadiaDataModel> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _estadiaRepository.GetByIdAsync(id, ct);
            return entity?.ToModel();
        }

        public async Task<EstadiaDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var entity = await _estadiaRepository.GetByGuidAsync(guid, ct);
            return entity?.ToModel();
        }

        public async Task<DataPagedResult<EstadiaDataModel>> GetByFiltroAsync(EstadiaFiltroDataModel filtro, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var all = await _estadiaRepository.GetAllAsync(ct);
            var query = all.AsQueryable();

            if (filtro.IdReservaHabitacion.HasValue)
                query = query.Where(e => e.IdReservaHabitacion == filtro.IdReservaHabitacion.Value);
            if (filtro.IdCliente.HasValue)
                query = query.Where(e => e.IdCliente == filtro.IdCliente.Value);
            if (filtro.IdHabitacion.HasValue)
                query = query.Where(e => e.IdHabitacion == filtro.IdHabitacion.Value);
            if (!string.IsNullOrEmpty(filtro.EstadoEstadia))
                query = query.Where(e => e.EstadoEstadia == filtro.EstadoEstadia);
            if (filtro.CheckinDesde.HasValue)
                query = query.Where(e => e.CheckinUtc >= filtro.CheckinDesde.Value);
            if (filtro.CheckinHasta.HasValue)
                query = query.Where(e => e.CheckinUtc <= filtro.CheckinHasta.Value);
            if (filtro.RequiereMantenimiento.HasValue)
                query = query.Where(e => e.RequiereMantenimiento == filtro.RequiereMantenimiento.Value);

            var totalCount = query.Count();
            var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new DataPagedResult<EstadiaDataModel>
            {
                Items = items.ToModelList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<EstadiaDataModel> AddAsync(EstadiaDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            if (entity.EstadiaGuid == Guid.Empty) entity.EstadiaGuid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.CreadoPorUsuario)) entity.CreadoPorUsuario = "Sistema";
            if (string.IsNullOrWhiteSpace(entity.ServicioOrigen)) entity.ServicioOrigen = "hospedaje-service";
            entity.FechaRegistroUtc = DateTime.UtcNow;
            var added = await _estadiaRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return added.ToModel();
        }

        public async Task UpdateAsync(EstadiaDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            await _estadiaRepository.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task RegistrarCheckoutAsync(int idEstadia, string observaciones, bool requiereMantenimiento, string usuario, CancellationToken ct = default)
        {
            await _estadiaRepository.RegistrarCheckoutAsync(idEstadia, observaciones, requiereMantenimiento, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<int> HacerCheckinAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            var result = await _estadiaRepository.HacerCheckinAsync(idReserva, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return result;
        }

        public async Task<IEnumerable<EstadiaDataModel>> GetByReservaAsync(int idReserva, CancellationToken ct = default)
        {
            return (await _estadiaRepository.GetByReservaAsync(idReserva, ct)).ToModelList();
        }

        public async Task<CargoEstadiaDataModel> AddCargoAsync(int idEstadia, CargoEstadiaDataModel cargo, CancellationToken ct = default)
        {
            var entity = cargo.ToEntity();
            entity.IdEstadia = idEstadia;
            if (entity.CargoGuid == Guid.Empty) entity.CargoGuid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.CreadoPorUsuario)) entity.CreadoPorUsuario = "Sistema";
            if (string.IsNullOrWhiteSpace(entity.ServicioOrigen)) entity.ServicioOrigen = "hospedaje-service";
            entity.FechaRegistroUtc = DateTime.UtcNow;
            var added = await _cargoEstadiaRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return added.ToModel();
        }

        public async Task<CargoEstadiaDataModel?> GetCargoByIdAsync(int idCargo, CancellationToken ct = default)
        {
            var entity = await _cargoEstadiaRepository.GetByIdAsync(idCargo, ct);
            return entity?.ToModel();
        }

        public async Task AnularCargoAsync(int idCargo, string usuario, CancellationToken ct = default)
        {
            await _cargoEstadiaRepository.AnularAsync(idCargo, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<DataPagedResult<CargoEstadiaDataModel>> GetCargosByEstadiaAsync(int idEstadia, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var all = await _cargoEstadiaRepository.GetByEstadiaAsync(idEstadia, ct);
            var items = all.ToModelList();
            var totalCount = items.Count;
            var pagedItems = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new DataPagedResult<CargoEstadiaDataModel>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
