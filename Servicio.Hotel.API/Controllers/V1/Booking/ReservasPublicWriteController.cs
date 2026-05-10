using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Models.Requests.Public;
using Servicio.Hotel.API.Models.Responses.Public;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using Servicio.Hotel.Business.Interfaces.Reservas;
using Servicio.Hotel.Business.Interfaces.Seguridad;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Booking
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/public/reservas")]
    public class ReservasPublicWriteController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IClienteService _clienteService;
        private readonly ISucursalService _sucursalService;
        private readonly IHabitacionService _habitacionService;

        public ReservasPublicWriteController(
            IReservaService reservaService,
            IUsuarioService usuarioService,
            IClienteService clienteService,
            ISucursalService sucursalService,
            IHabitacionService habitacionService)
        {
            _reservaService = reservaService;
            _usuarioService = usuarioService;
            _clienteService = clienteService;
            _sucursalService = sucursalService;
            _habitacionService = habitacionService;
        }

        [HttpGet("{reservaGuid:guid}")]
        public async Task<ActionResult<ReservaPublicDto>> GetByGuid(Guid reservaGuid)
        {
            var result = await _reservaService.GetByGuidAsync(reservaGuid);
            return Ok(await ToPublicReservaDtoAsync(result));
        }

        [HttpGet]
        public async Task<ActionResult> GetMisReservas(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 50,
            [FromQuery] string? estado = null)
        {
            var idCliente = await GetAuthenticatedClienteIdAsync();
            var filtro = new ReservaFiltroDTO 
            { 
                IdCliente = idCliente,
                EstadoReserva = estado ?? string.Empty
            };
            var result = await _reservaService.GetByFiltroAsync(filtro, page, limit);
            var items = new List<ReservaPublicDto>();
            foreach (var reserva in result.Items)
            {
                items.Add(await ToPublicReservaDtoAsync(reserva));
            }

            return Ok(new
            {
                items,
                result.TotalCount,
                result.PageNumber,
                result.PageSize
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ReservaPublicDto>> Create([FromBody] PublicReservaCreateRequest request)
        {
            request.ValidateNoIds();

            if (request.FechaFin <= request.FechaInicio)
                throw new ValidationException("RES-PUB-001", "La fecha de fin debe ser posterior a la fecha de inicio.");

            if (request.Habitaciones == null || request.Habitaciones.Count == 0)
                throw new ValidationException("RES-PUB-002", "La reserva debe tener al menos una habitacion.");

            if (request.ClienteGuid == Guid.Empty && request.Cliente == null)
                throw new ValidationException("RES-PUB-003", "clienteGuid o cliente es obligatorio.");

            if (request.SucursalGuid == Guid.Empty)
                throw new ValidationException("RES-PUB-004", "sucursalGuid es obligatorio.");

            var cliente = request.ClienteGuid != Guid.Empty
                ? await _clienteService.GetByGuidAsync(request.ClienteGuid)
                : await GetOrCreateClienteAsync(request.Cliente!);
            var sucursal = await _sucursalService.GetByGuidAsync(request.SucursalGuid);

            var habitaciones = new List<ReservaTipoHabitacionCreateDTO>();
            foreach (var habitacionRequest in request.Habitaciones)
            {
                if (habitacionRequest.TipoHabitacionGuid == Guid.Empty)
                    throw new ValidationException("RES-PUB-005", "tipoHabitacionGuid es obligatorio para cada habitacion.");

                habitaciones.Add(new ReservaTipoHabitacionCreateDTO
                {
                    TipoHabitacionGuid = habitacionRequest.TipoHabitacionGuid,
                    NumHabitaciones = habitacionRequest.NumHabitaciones,
                    NumAdultos = habitacionRequest.NumAdultos,
                    NumNinos = habitacionRequest.NumNinos
                });
            }

            var createDto = new ReservaPorTipoHabitacionCreateDTO
            {
                IdCliente = cliente.IdCliente,
                IdSucursal = sucursal.IdSucursal,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                DescuentoAplicado = request.DescuentoAplicado,
                OrigenCanalReserva = string.IsNullOrWhiteSpace(request.OrigenCanalReserva) ? "API_PUBLICA" : request.OrigenCanalReserva,
                Observaciones = request.Observaciones ?? string.Empty,
                EsWalkin = request.EsWalkin,
                Habitaciones = habitaciones
            };

            var result = await _reservaService.CreateByTipoHabitacionAsync(createDto, HttpContext.RequestAborted);
            var response = await ToPublicReservaDtoAsync(result);
            return CreatedAtAction(nameof(GetByGuid), new { reservaGuid = response.ReservaGuid }, response);
        }

        private async Task<ClienteDTO> GetOrCreateClienteAsync(PublicClienteCreateRequest clienteRequest)
        {
            if (string.IsNullOrWhiteSpace(clienteRequest.TipoIdentificacion) ||
                string.IsNullOrWhiteSpace(clienteRequest.NumeroIdentificacion) ||
                string.IsNullOrWhiteSpace(clienteRequest.Nombres) ||
                string.IsNullOrWhiteSpace(clienteRequest.Correo) ||
                string.IsNullOrWhiteSpace(clienteRequest.Telefono))
            {
                throw new ValidationException("RES-PUB-CLI-001", "tipoIdentificacion, numeroIdentificacion, nombres, correo y telefono son obligatorios.");
            }

            try
            {
                return await _clienteService.GetByIdentificacionAsync(clienteRequest.TipoIdentificacion, clienteRequest.NumeroIdentificacion, HttpContext.RequestAborted);
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

        [HttpPatch("{reservaGuid:guid}/cancelar")]
        [AllowAnonymous]
        public async Task<IActionResult> Cancelar(Guid reservaGuid, [FromBody] PublicCancelarReservaRequest request)
        {
            if (reservaGuid == Guid.Empty)
                throw new ValidationException("RES-PUB-CAN-001", "reservaGuid es obligatorio.");

            request.ValidateNoIds();
            var reserva = await _reservaService.GetByGuidAsync(reservaGuid);
            await _reservaService.CancelarAsync(
                reserva.IdReserva,
                string.IsNullOrWhiteSpace(request.Motivo) ? "Cancelada desde flujo publico." : request.Motivo,
                User.Identity?.Name ?? "CLIENTE_PUBLICO",
                HttpContext.RequestAborted);

            return NoContent();
        }

        [HttpPost("calcular-precio")]
        [AllowAnonymous]
        public async Task<ActionResult<ReservaPrecioPublicDto>> CalcularPrecio([FromBody] PublicReservaPrecioRequest request)
        {
            request.ValidateNoIds();

            if (request.HabitacionGuid == Guid.Empty)
                throw new ValidationException("RES-PRECIO-PUB-001", "habitacionGuid es obligatorio.");

            if (request.FechaFin <= request.FechaInicio)
                throw new ValidationException("RES-PRECIO-PUB-002", "La fecha de fin debe ser posterior a la fecha de inicio.");

            var habitacion = await _habitacionService.GetByGuidAsync(request.HabitacionGuid);
            var precio = await _reservaService.CalcularPrecioHabitacionAsync(
                habitacion.IdHabitacion,
                request.FechaInicio,
                request.FechaFin,
                request.Canal ?? "WEB",
                HttpContext.RequestAborted);

            return Ok(new ReservaPrecioPublicDto
            {
                HabitacionGuid = habitacion.HabitacionGuid,
                PrecioNocheAplicado = precio.PrecioNocheAplicado,
                SubtotalLinea = precio.SubtotalLinea,
                ValorIvaLinea = precio.ValorIvaLinea,
                TotalLinea = precio.TotalLinea,
                OrigenPrecio = precio.OrigenPrecio
            });
        }

        private async Task<ReservaPublicDto> ToPublicReservaDtoAsync(ReservaDTO reserva)
        {
            var cliente = await _clienteService.GetByIdAsync(reserva.IdCliente);
            var sucursal = await _sucursalService.GetByIdAsync(reserva.IdSucursal);
            var habitaciones = new List<ReservaHabitacionPublicDto>();

            foreach (var detalle in reserva.Habitaciones ?? new List<ReservaHabitacionDTO>())
            {
                var habitacion = await _habitacionService.GetByIdAsync(detalle.IdHabitacion);
                habitaciones.Add(new ReservaHabitacionPublicDto
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

            return new ReservaPublicDto
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
                FechaCancelacionUtc = reserva.FechaCancelacionUtc,
                MotivoCancelacion = reserva.MotivoCancelacion,
                Observaciones = reserva.Observaciones,
                EsWalkin = reserva.EsWalkin,
                Habitaciones = habitaciones
            };
        }

        private async Task<int> GetAuthenticatedClienteIdAsync()
        {
            var idClienteClaim = User.Claims.FirstOrDefault(c => c.Type == "idCliente")?.Value;
            if (int.TryParse(idClienteClaim, out var idCliente) && idCliente > 0)
                return idCliente;

            var idUsuarioClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idUsuarioClaim, out var idUsuario))
                throw new UnauthorizedBusinessException("AUTH-CLIENTE-001", "Token sin identificacion de usuario.");

            var usuario = await _usuarioService.GetByIdAsync(idUsuario);
            if (usuario.IdCliente.HasValue && usuario.IdCliente.Value > 0)
                return usuario.IdCliente.Value;

            var cliente = await _clienteService.CreateAsync(new ClienteCreateDTO
            {
                TipoIdentificacion = "CLI",
                NumeroIdentificacion = $"CLI-{usuario.IdUsuario:D6}",
                Nombres = usuario.Nombres ?? string.Empty,
                Apellidos = usuario.Apellidos ?? string.Empty,
                RazonSocial = string.Empty,
                Correo = usuario.Correo ?? string.Empty,
                Telefono = "0000000000",
                Direccion = string.Empty,
                Estado = "ACT",
            });

            await _usuarioService.AsociarClienteAsync(idUsuario, cliente.IdCliente, User.Identity?.Name ?? usuario.Username, HttpContext.RequestAborted);
            return cliente.IdCliente;
        }
    }
}
