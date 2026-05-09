using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Hospedaje;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Hospedaje;
using Servicio.Hotel.Business.Mappers.Hospedaje;
using Servicio.Hotel.DataManagement.Hospedaje.Interfaces;

namespace Servicio.Hotel.Business.Services.Hospedaje
{
    public class EstadiaService : IEstadiaService
    {
        private readonly IEstadiaDataService _estadiaDataService;
        private readonly Servicio.Hotel.DataManagement.Reservas.Interfaces.IReservaDataService _reservaDataService;

        public EstadiaService(
            IEstadiaDataService estadiaDataService,
            Servicio.Hotel.DataManagement.Reservas.Interfaces.IReservaDataService reservaDataService)
        {
            _estadiaDataService = estadiaDataService;
            _reservaDataService = reservaDataService;
        }

        public async Task<EstadiaDTO> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var dataModel = await _estadiaDataService.GetByIdAsync(id, ct);
            if (dataModel == null)
                throw new NotFoundException("EST-001", $"No se encontró la estadía con ID {id}.");
            return dataModel.ToDto();
        }

        public async Task<EstadiaDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var dataModel = await _estadiaDataService.GetByGuidAsync(guid, ct);
            if (dataModel == null)
                throw new NotFoundException("EST-002", $"No se encontró la estadía con GUID {guid}.");
            return dataModel.ToDto();
        }

        public async Task<PagedResult<EstadiaDTO>> GetByFiltroAsync(EstadiaFiltroDTO filtro, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var pagedData = await _estadiaDataService.GetByFiltroAsync(filtro.ToDataModel(), pageNumber, pageSize, ct);
            return new PagedResult<EstadiaDTO>
            {
                Items = pagedData.Items.ToDtoList(),
                TotalCount = pagedData.TotalCount,
                PageNumber = pagedData.PageNumber,
                PageSize = pagedData.PageSize
            };
        }

        public async Task<EstadiaDTO> CreateAsync(EstadiaDTO estadiaDto, CancellationToken ct = default)
        {
            if (estadiaDto.IdReservaHabitacion <= 0)
                throw new ValidationException("EST-003", "El ID de reserva habitación es obligatorio.");
            var dataModel = estadiaDto.ToDataModel();
            var created = await _estadiaDataService.AddAsync(dataModel, ct);
            return created.ToDto();
        }

        public async Task UpdateAsync(EstadiaDTO estadiaDto, CancellationToken ct = default)
        {
            var existing = await _estadiaDataService.GetByIdAsync(estadiaDto.IdEstadia, ct);
            if (existing == null)
                throw new NotFoundException("EST-004", $"No se encontró la estadía con ID {estadiaDto.IdEstadia}.");
            var dataModel = estadiaDto.ToDataModel();
            await _estadiaDataService.UpdateAsync(dataModel, ct);
        }

        public async Task RegistrarCheckoutAsync(int idEstadia, string observaciones, bool requiereMantenimiento, string usuario, CancellationToken ct = default)
        {
            var existing = await _estadiaDataService.GetByIdAsync(idEstadia, ct);
            if (existing == null)
                throw new NotFoundException("EST-005", $"No se encontró la estadía con ID {idEstadia}.");
            await _estadiaDataService.RegistrarCheckoutAsync(idEstadia, observaciones, requiereMantenimiento, usuario, ct);
        }

        public async Task<IEnumerable<EstadiaDTO>> HacerCheckinAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            var reserva = await _reservaDataService.GetByIdAsync(idReserva, ct);
            if (reserva == null)
                throw new NotFoundException("RES-001", $"No se encontró la reserva con ID {idReserva}.");

            if (!string.Equals(reserva.EstadoReserva?.Trim(), "CON", StringComparison.Ordinal))
                throw new ConflictException("La reserva debe estar en estado CON para realizar el check-in.");

            var existentes = await _estadiaDataService.GetByReservaAsync(idReserva, ct);
            if (existentes != null && existentes.Any())
                throw new ConflictException("Ya existe un check-in registrado para esta reserva.");

            await _estadiaDataService.HacerCheckinAsync(idReserva, usuario, ct);

            var estadias = await _estadiaDataService.GetByReservaAsync(idReserva, ct);
            var dtos = estadias.ToDtoList();

            if (dtos.Count == 0)
                throw new ConflictException("No se generaron estadías para la reserva especificada.");

            return dtos;
        }

        public async Task<CargoEstadiaDTO> AddCargoAsync(int idEstadia, CargoEstadiaDTO cargoDto, CancellationToken ct = default)
        {
            var estadia = await _estadiaDataService.GetByIdAsync(idEstadia, ct);
            if (estadia == null)
                throw new NotFoundException("EST-006", $"No se encontró la estadía con ID {idEstadia}.");
            if (cargoDto.PrecioUnitario <= 0)
                throw new ValidationException("EST-007", "El precio unitario del cargo debe ser mayor a cero.");
            var cargoData = cargoDto.ToDataModel();
            var created = await _estadiaDataService.AddCargoAsync(idEstadia, cargoData, ct);
            return created.ToDto();
        }

        public async Task AnularCargoAsync(int idCargo, string usuario, CancellationToken ct = default)
        {
            var cargo = await _estadiaDataService.GetCargoByIdAsync(idCargo, ct);
            if (cargo == null)
                throw new NotFoundException($"No se encontró el cargo de estadía con ID {idCargo}.");

            if (cargo.EstadoCargo == "ANU")
                throw new ConflictException("El cargo ya está anulado.");

            if (cargo.EstadoCargo == "FAC")
                throw new ConflictException("No se puede anular un cargo ya facturado.");

            if (cargo.EstadoCargo != "PEN")
                throw new ConflictException($"No se puede anular un cargo en estado '{cargo.EstadoCargo}'.");

            await _estadiaDataService.AnularCargoAsync(idCargo, usuario, ct);
        }

        public async Task<PagedResult<CargoEstadiaDTO>> GetCargosByEstadiaAsync(int idEstadia, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var pagedData = await _estadiaDataService.GetCargosByEstadiaAsync(idEstadia, pageNumber, pageSize, ct);
            return new PagedResult<CargoEstadiaDTO>
            {
                Items = pagedData.Items.ToDtoList(),
                TotalCount = pagedData.TotalCount,
                PageNumber = pagedData.PageNumber,
                PageSize = pagedData.PageSize
            };
        }
    }
}
