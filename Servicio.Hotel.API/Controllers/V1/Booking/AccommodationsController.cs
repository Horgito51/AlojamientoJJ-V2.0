using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Models.Requests.Public;
using Servicio.Hotel.Business.DTOs.Booking;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using Servicio.Hotel.Business.Interfaces.Booking;
using Servicio.Hotel.Business.Interfaces.Reservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/accommodations")]
    public class AccommodationsController : ControllerBase
    {
        private readonly IBookingAccommodationService _bookingService;
        private readonly IReservaService _reservaService;
        private readonly IClienteService _clienteService;
        private readonly ISucursalService _sucursalService;
        private readonly IHabitacionService _habitacionService;

        public AccommodationsController(
            IBookingAccommodationService bookingService,
            IReservaService reservaService,
            IClienteService clienteService,
            ISucursalService sucursalService,
            IHabitacionService habitacionService)
        {
            _bookingService = bookingService;
            _reservaService = reservaService;
            _clienteService = clienteService;
            _sucursalService = sucursalService;
            _habitacionService = habitacionService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<BookingPagedResponseDTO<AccommodationSearchItemDTO>>> Search(
            [FromQuery] AccommodationSearchQueryDTO query)
        {
            RejectIdQueryParameters();
            return Ok(await _bookingService.SearchAsync(query, HttpContext.RequestAborted));
        }

        [HttpGet("{sucursalGuid:guid}")]
        public async Task<ActionResult<AccommodationDetailResponseDTO>> GetByGuid(
            Guid sucursalGuid,
            [FromQuery] DateTime? fechaEntrada = null,
            [FromQuery] DateTime? fechaSalida = null)
        {
            return Ok(await _bookingService.GetDetailAsync(sucursalGuid, fechaEntrada, fechaSalida, HttpContext.RequestAborted));
        }

        [HttpGet("{sucursalGuid:guid}/reviews")]
        public async Task<ActionResult<BookingPagedResponseDTO<AccommodationReviewDTO>>> GetReviews(
            Guid sucursalGuid,
            [FromQuery] int pagina = 1,
            [FromQuery] int limite = 10)
        {
            return Ok(await _bookingService.GetReviewsAsync(sucursalGuid, pagina, limite, HttpContext.RequestAborted));
        }

        [HttpGet("sucursales/{sucursalGuid:guid}/habitaciones")]
        public async Task<ActionResult<IEnumerable<HabitacionPublicListItemDTO>>> GetHabitacionesDisponibles(
            Guid sucursalGuid,
            [FromQuery] DateTime fechaEntrada,
            [FromQuery] DateTime fechaSalida)
        {
            RejectIdQueryParameters();
            return Ok(await _bookingService.GetHabitacionesDisponiblesAsync(sucursalGuid, fechaEntrada, fechaSalida, HttpContext.RequestAborted));
        }

        [HttpPost("reservas")]
        [AllowAnonymous]
        public async Task<ActionResult<ReservaPublicContractDTO>> CreateReserva([FromBody] CrearReservaPublicRequestDTO request)
        {
            ValidateCreateReservationRequest(request);

            var cliente = await GetOrCreateClienteAsync(request);
            var sucursal = await _sucursalService.GetByGuidAsync(request.SucursalGuid, HttpContext.RequestAborted);

            var createDto = new ReservaPorTipoHabitacionCreateDTO
            {
                IdCliente = cliente.IdCliente,
                IdSucursal = sucursal.IdSucursal,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                OrigenCanalReserva = string.IsNullOrWhiteSpace(request.OrigenCanalReserva) ? "MARKETPLACE" : request.OrigenCanalReserva,
                Observaciones = request.Observaciones ?? string.Empty,
                EsWalkin = false,
                Habitaciones = request.Habitaciones.Select(h => new ReservaTipoHabitacionCreateDTO
                {
                    TipoHabitacionGuid = h.TipoHabitacionGuid,
                    NumHabitaciones = h.NumHabitaciones,
                    NumAdultos = h.NumAdultos,
                    NumNinos = h.NumNinos
                }).ToList()
            };

            var created = await _reservaService.CreateByTipoHabitacionAsync(createDto, HttpContext.RequestAborted);
            var response = await ToBookingReservaDtoAsync(created);

            return CreatedAtAction(nameof(GetReserva), new { reservaGuid = response.ReservaGuid, clienteGuid = response.ClienteGuid }, response);
        }

        [HttpGet("reservas/{reservaGuid:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<ReservaPublicContractDTO>> GetReserva(
            Guid reservaGuid,
            [FromQuery] Guid clienteGuid)
        {
            if (reservaGuid == Guid.Empty)
                throw new ValidationException("RES-BOOK-011", "reservaGuid es obligatorio.");
            if (clienteGuid == Guid.Empty)
                throw new ValidationException("RES-BOOK-012", "clienteGuid es obligatorio.");

            var reserva = await _reservaService.GetByGuidAsync(reservaGuid, HttpContext.RequestAborted);
            var cliente = await _clienteService.GetByGuidAsync(clienteGuid, HttpContext.RequestAborted);

            if (reserva.IdCliente != cliente.IdCliente)
                throw new UnauthorizedBusinessException("RES-BOOK-013", "La reserva no pertenece al cliente indicado.");

            return Ok(await ToBookingReservaDtoAsync(reserva));
        }

        private static void ValidateCreateReservationRequest(CrearReservaPublicRequestDTO request)
        {
            if (request == null)
                throw new ValidationException("RES-BOOK-001", "El cuerpo de la reserva es obligatorio.");
            if (request.SucursalGuid == Guid.Empty)
                throw new ValidationException("RES-BOOK-002", "sucursalGuid es obligatorio.");
            if (request.FechaInicio == default)
                throw new ValidationException("RES-BOOK-003", "fechaInicio es obligatoria.");
            if (request.FechaFin == default || request.FechaFin <= request.FechaInicio)
                throw new ValidationException("RES-BOOK-004", "fechaFin debe ser posterior a fechaInicio.");
            if ((!request.ClienteGuid.HasValue || request.ClienteGuid.Value == Guid.Empty) && request.Cliente == null)
                throw new ValidationException("RES-BOOK-005", "cliente es obligatorio.");
            if (request.Habitaciones == null || request.Habitaciones.Count == 0)
                throw new ValidationException("RES-BOOK-006", "La reserva debe incluir al menos un tipo de habitacion.");

            foreach (var habitacion in request.Habitaciones)
            {
                if (habitacion.TipoHabitacionGuid == Guid.Empty)
                    throw new ValidationException("RES-BOOK-007", "tipoHabitacionGuid es obligatorio.");
                if (habitacion.NumHabitaciones <= 0 || habitacion.NumAdultos <= 0 || habitacion.NumNinos < 0)
                    throw new ValidationException("RES-BOOK-008", "numHabitaciones y numAdultos deben ser positivos; numNinos no puede ser negativo.");
            }
        }

        private async Task<ClienteDTO> GetOrCreateClienteAsync(CrearReservaPublicRequestDTO request)
        {
            if (request.ClienteGuid.HasValue && request.ClienteGuid.Value != Guid.Empty)
                return await _clienteService.GetByGuidAsync(request.ClienteGuid.Value, HttpContext.RequestAborted);

            var clienteRequest = request.Cliente;
            if (clienteRequest == null ||
                string.IsNullOrWhiteSpace(clienteRequest.TipoIdentificacion) ||
                string.IsNullOrWhiteSpace(clienteRequest.NumeroIdentificacion) ||
                string.IsNullOrWhiteSpace(clienteRequest.Nombres) ||
                string.IsNullOrWhiteSpace(clienteRequest.Correo) ||
                string.IsNullOrWhiteSpace(clienteRequest.Telefono))
            {
                throw new ValidationException("RES-BOOK-015", "tipoIdentificacion, numeroIdentificacion, nombres, correo y telefono son obligatorios.");
            }

            try
            {
                return await _clienteService.GetByIdentificacionAsync(
                    clienteRequest.TipoIdentificacion,
                    clienteRequest.NumeroIdentificacion,
                    HttpContext.RequestAborted);
            }
            catch (NotFoundException)
            {
                try
                {
                    return await _clienteService.GetByCorreoAsync(clienteRequest.Correo, HttpContext.RequestAborted);
                }
                catch (NotFoundException)
                {
                    return await _clienteService.CreateAsync(new ClienteCreateDTO
                    {
                        TipoIdentificacion = clienteRequest.TipoIdentificacion,
                        NumeroIdentificacion = clienteRequest.NumeroIdentificacion,
                        Nombres = clienteRequest.Nombres,
                        Apellidos = clienteRequest.Apellidos ?? string.Empty,
                        RazonSocial = string.Empty,
                        Correo = clienteRequest.Correo,
                        Telefono = clienteRequest.Telefono,
                        Direccion = clienteRequest.Direccion ?? string.Empty,
                        Estado = "ACT"
                    }, HttpContext.RequestAborted);
                }
            }
        }

        private async Task<ReservaPublicContractDTO> ToBookingReservaDtoAsync(ReservaDTO reserva)
        {
            var cliente = await _clienteService.GetByIdAsync(reserva.IdCliente, HttpContext.RequestAborted);
            var sucursal = await _sucursalService.GetByIdAsync(reserva.IdSucursal, HttpContext.RequestAborted);
            var habitaciones = new List<ReservaHabitacionPublicContractDTO>();

            foreach (var detalle in reserva.Habitaciones ?? new List<ReservaHabitacionDTO>())
            {
                var habitacion = await _habitacionService.GetByIdAsync(detalle.IdHabitacion, HttpContext.RequestAborted);
                habitaciones.Add(new ReservaHabitacionPublicContractDTO
                {
                    ReservaHabitacionGuid = detalle.ReservaHabitacionGuid,
                    HabitacionGuid = habitacion.HabitacionGuid,
                    FechaInicio = detalle.FechaInicio,
                    FechaFin = detalle.FechaFin,
                    NumAdultos = detalle.NumAdultos,
                    NumNinos = detalle.NumNinos,
                    PrecioNocheAplicado = detalle.PrecioNocheAplicado,
                    SubtotalLinea = detalle.SubtotalLinea,
                    ValorIvaLinea = detalle.ValorIvaLinea,
                    DescuentoLinea = detalle.DescuentoLinea,
                    TotalLinea = detalle.TotalLinea,
                    EstadoDetalle = detalle.EstadoDetalle
                });
            }

            return new ReservaPublicContractDTO
            {
                ReservaGuid = reserva.GuidReserva,
                CodigoReserva = reserva.CodigoReserva,
                ClienteGuid = cliente.ClienteGuid,
                SucursalGuid = sucursal.SucursalGuid,
                FechaReservaUtc = reserva.FechaReservaUtc,
                FechaInicio = reserva.FechaInicio,
                FechaFin = reserva.FechaFin,
                SubtotalReserva = reserva.SubtotalReserva,
                ValorIva = reserva.ValorIva,
                TotalReserva = reserva.TotalReserva,
                DescuentoAplicado = reserva.DescuentoAplicado,
                SaldoPendiente = reserva.SaldoPendiente,
                OrigenCanalReserva = reserva.OrigenCanalReserva,
                EstadoReserva = reserva.EstadoReserva,
                FechaConfirmacionUtc = reserva.FechaConfirmacionUtc,
                Observaciones = reserva.Observaciones,
                EsWalkin = reserva.EsWalkin,
                Habitaciones = habitaciones
            };
        }

        private void RejectIdQueryParameters()
        {
            foreach (var key in Request.Query.Keys)
            {
                if (PublicRequestGuard.IsIdProperty(key))
                    throw new ValidationException("PUB-GUID-QUERY-001", $"El parametro '{key}' no esta permitido en endpoints publicos. Use GUIDs.");
            }
        }
    }
}
