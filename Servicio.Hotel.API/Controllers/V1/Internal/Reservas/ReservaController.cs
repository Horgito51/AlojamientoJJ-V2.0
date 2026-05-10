using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Servicio.Hotel.API.Models.Requests.Internal;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using Servicio.Hotel.Business.Interfaces.Reservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Internal.Reservas
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/internal/reservas")]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly IClienteService _clienteService;
        private readonly ISucursalService _sucursalService;

        public ReservaController(IReservaService reservaService, IClienteService clienteService, ISucursalService sucursalService)
        {
            _reservaService = reservaService;
            _clienteService = clienteService;
            _sucursalService = sucursalService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaDTO>>> GetAll()
        {
            var pagedResult = await _reservaService.GetByFiltroAsync(new ReservaFiltroDTO(), 1, 50);
            return Ok(pagedResult.Items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaDTO>> GetById(int id)
        {
            var result = await _reservaService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ReservaDTO>> Create([FromBody] ReservaCreateRequest request)
        {
            var hasTipoHabitacion = request.Habitaciones.Any(h => h.TipoHabitacionGuid.HasValue && h.TipoHabitacionGuid.Value != Guid.Empty);
            ReservaDTO result;

            if (hasTipoHabitacion)
            {
                var idCliente = await ResolveClienteIdAsync(request);
                var idSucursal = await ResolveSucursalIdAsync(request);
                var dto = new ReservaPorTipoHabitacionCreateDTO
                {
                    IdCliente = idCliente,
                    IdSucursal = idSucursal,
                    FechaInicio = request.FechaInicio,
                    FechaFin = request.FechaFin,
                    DescuentoAplicado = request.DescuentoAplicado,
                    OrigenCanalReserva = string.IsNullOrWhiteSpace(request.OrigenCanalReserva) ? "INTERNAL" : request.OrigenCanalReserva,
                    Observaciones = request.Observaciones ?? string.Empty,
                    EsWalkin = request.EsWalkin,
                    ExigirPermiteReservaPublica = false,
                    Habitaciones = request.Habitaciones.Select(h => new ReservaTipoHabitacionCreateDTO
                    {
                        TipoHabitacionGuid = h.TipoHabitacionGuid ?? Guid.Empty,
                        NumHabitaciones = h.NumHabitaciones,
                        NumAdultos = h.NumAdultos,
                        NumNinos = h.NumNinos
                    }).ToList()
                };
                result = await _reservaService.CreateByTipoHabitacionAsync(dto, HttpContext.RequestAborted);
            }
            else
            {
                var dto = request.ToCreateDto();
                result = await _reservaService.CreateAsync(dto);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.IdReserva }, result);
        }

        private async Task<int> ResolveClienteIdAsync(ReservaCreateRequest request)
        {
            if (request.IdCliente > 0)
                return request.IdCliente;
            if (request.ClienteGuid.HasValue && request.ClienteGuid.Value != Guid.Empty)
                return (await _clienteService.GetByGuidAsync(request.ClienteGuid.Value, HttpContext.RequestAborted)).IdCliente;
            if (request.Cliente == null)
                throw new ValidationException("RES-INT-CLI-001", "IdCliente, clienteGuid o cliente es obligatorio.");

            try
            {
                return (await _clienteService.GetByIdentificacionAsync(
                    request.Cliente.TipoIdentificacion,
                    request.Cliente.NumeroIdentificacion,
                    HttpContext.RequestAborted)).IdCliente;
            }
            catch (NotFoundException)
            {
                return (await _clienteService.CreateAsync(request.Cliente.ToCreateDto(), HttpContext.RequestAborted)).IdCliente;
            }
        }

        private async Task<int> ResolveSucursalIdAsync(ReservaCreateRequest request)
        {
            if (request.IdSucursal > 0)
                return request.IdSucursal;
            if (request.SucursalGuid.HasValue && request.SucursalGuid.Value != Guid.Empty)
                return (await _sucursalService.GetByGuidAsync(request.SucursalGuid.Value, HttpContext.RequestAborted)).IdSucursal;
            throw new ValidationException("RES-INT-SUC-001", "IdSucursal o sucursalGuid es obligatorio.");
        }

        [HttpPost("calcular-precio")]
        public async Task<ActionResult<ReservaPrecioDTO>> CalcularPrecio([FromBody] ReservaPrecioRequest request)
        {
            var result = await _reservaService.CalcularPrecioHabitacionAsync(
                request.IdHabitacion,
                request.FechaInicio,
                request.FechaFin,
                request.Canal,
                HttpContext.RequestAborted);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReservaUpdateRequest request)
        {
            var dto = request.ToUpdateDto(id);
            await _reservaService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpPatch("{id}/confirmar")]
        public async Task<IActionResult> Confirm(int id)
        {
            await _reservaService.ConfirmarAsync(id, "Sistema");
            return NoContent();
        }

        [HttpPatch("{id}/cancelar")]
        public async Task<IActionResult> Cancel(int id, [FromBody] CancelarReservaRequest request)
        {
            await _reservaService.CancelarAsync(id, request.Motivo, "Sistema");
            return NoContent();
        }
    }
}
