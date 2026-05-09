using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Servicio.Hotel.API.Models.Requests.Internal;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.Business.Interfaces.Reservas;
using System.Collections.Generic;
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

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
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
            var dto = request.ToCreateDto();
            var result = await _reservaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.IdReserva }, result);
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
