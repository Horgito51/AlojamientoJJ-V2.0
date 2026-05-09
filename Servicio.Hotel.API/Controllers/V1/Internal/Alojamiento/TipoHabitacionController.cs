using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Models.Requests.Internal;
using Servicio.Hotel.API.Models.Common;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Internal.Alojamiento
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/internal/tipos-habitacion")]
    public class TipoHabitacionController : ControllerBase
    {
        private readonly ITipoHabitacionService _tipoHabitacionService;

        public TipoHabitacionController(ITipoHabitacionService tipoHabitacionService)
        {
            _tipoHabitacionService = tipoHabitacionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoHabitacionDTO>>> GetAll([FromQuery] string? estado = null)
        {
            if (!string.IsNullOrWhiteSpace(estado) && estado != "ACT" && estado != "INA")
                return BadRequest(ApiErrorResponse.BadRequest("El parámetro estado es inválido. Use: ACT o INA.", null, HttpContext.TraceIdentifier));

            var result = await _tipoHabitacionService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(estado))
                return Ok(result.Where(t => t.EstadoTipoHabitacion == estado));

            return Ok(result.Where(t => t.EstadoTipoHabitacion != "INA"));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TipoHabitacionDTO>> GetById(int id)
        {
            var result = await _tipoHabitacionService.GetByIdAsync(id);
            return Ok(result);
        }

        // Soporta: /tipos-habitacion/{tipo_guid} y /tipos-habitacion/{slug}
        [HttpGet("{param}")]
        public async Task<ActionResult<TipoHabitacionDTO>> GetByGuidOrSlug(string param)
        {
            if (Guid.TryParse(param, out var guid))
            {
                var byGuid = await _tipoHabitacionService.GetByGuidAsync(guid);
                return Ok(byGuid);
            }

            var bySlug = await _tipoHabitacionService.GetBySlugAsync(param);
            return Ok(bySlug);
        }

        [HttpPost]
        public async Task<ActionResult<TipoHabitacionDTO>> Create([FromBody] TipoHabitacionUpsertRequest request)
        {
            var dto = request.ToCreateDto();
            var result = await _tipoHabitacionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.IdTipoHabitacion }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TipoHabitacionUpsertRequest request)
        {
            var dto = request.ToUpdateDto(id);
            await _tipoHabitacionService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpPut("{tipoGuid:guid}")]
        public async Task<ActionResult<TipoHabitacionDTO>> UpdateByGuid(Guid tipoGuid, [FromBody] TipoHabitacionUpsertRequest request)
        {
            var existing = await _tipoHabitacionService.GetByGuidAsync(tipoGuid);
            var dto = request.ToUpdateDto(existing.IdTipoHabitacion);
            await _tipoHabitacionService.UpdateAsync(dto);
            var updated = await _tipoHabitacionService.GetByGuidAsync(tipoGuid);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tipoHabitacionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
