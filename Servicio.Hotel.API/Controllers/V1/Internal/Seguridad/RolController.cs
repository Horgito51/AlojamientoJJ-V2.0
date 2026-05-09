using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Servicio.Hotel.API.Models.Requests.Internal;
using Servicio.Hotel.API.Models.Common;
using Servicio.Hotel.Business.DTOs.Seguridad;
using Servicio.Hotel.Business.Interfaces.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Internal.Seguridad
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/internal/roles")]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolDTO>>> GetAll([FromQuery] string? estado = null)
        {
            if (!string.IsNullOrWhiteSpace(estado) && estado != "ACT" && estado != "INA")
                return BadRequest(ApiErrorResponse.BadRequest("El parámetro estado es inválido. Use: ACT o INA.", null, HttpContext.TraceIdentifier));

            var pagedResult = await _rolService.GetAllPagedAsync(1, 50);

            var items = pagedResult.Items;
            if (!string.IsNullOrWhiteSpace(estado))
                items = items.Where(r => r.EstadoRol == estado).ToList();
            else
                items = items.Where(r => r.EstadoRol != "INA").ToList();

            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RolDTO>> GetById(int id)
        {
            var rol = await _rolService.GetByIdAsync(id);
            return Ok(rol);
        }

        [HttpGet("{rolGuid:guid}")]
        public async Task<ActionResult<RolDTO>> GetByGuid(Guid rolGuid)
        {
            var rol = await _rolService.GetByGuidAsync(rolGuid);
            return Ok(rol);
        }

        [HttpPost]
        public async Task<ActionResult<RolDTO>> Create([FromBody] RolUpsertRequest request)
        {
            var rolCreateDto = request.ToCreateDto();
            var nuevoRol = await _rolService.CreateAsync(rolCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = nuevoRol.IdRol }, nuevoRol);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] RolUpsertRequest request)
        {
            var rolUpdateDto = request.ToUpdateDto(id);
            await _rolService.UpdateAsync(rolUpdateDto);
            return NoContent();
        }

        [HttpPut("{rolGuid:guid}")]
        public async Task<ActionResult<RolDTO>> UpdateByGuid(Guid rolGuid, [FromBody] RolUpsertRequest request)
        {
            var existing = await _rolService.GetByGuidAsync(rolGuid);
            var rolUpdateDto = request.ToUpdateDto(existing.IdRol);
            await _rolService.UpdateAsync(rolUpdateDto);
            var updated = await _rolService.GetByGuidAsync(rolGuid);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _rolService.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("{rolGuid:guid}")]
        public async Task<IActionResult> DeleteByGuid(Guid rolGuid)
        {
            var existing = await _rolService.GetByGuidAsync(rolGuid);
            await _rolService.DeleteAsync(existing.IdRol);
            return NoContent();
        }
    }
}
