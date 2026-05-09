using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Servicio.Hotel.API.Models.Requests.Internal;
using Servicio.Hotel.API.Models.Common;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Internal.Alojamiento
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/internal/sucursales")]
    public class SucursalController : ControllerBase
    {
        private readonly ISucursalService _service;

        public SucursalController(ISucursalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SucursalDTO>>> GetAll([FromQuery] string? estado = null)
        {
            if (!string.IsNullOrWhiteSpace(estado) && estado != "ACT" && estado != "INA")
                return BadRequest(ApiErrorResponse.BadRequest("El parámetro estado es inválido. Use: ACT o INA.", null, HttpContext.TraceIdentifier));

            return Ok(await _service.GetAllAsync(estado));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SucursalDTO>> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet("{sucursalGuid:guid}")]
        public async Task<ActionResult<SucursalDTO>> GetByGuid(Guid sucursalGuid)
            => Ok(await _service.GetByGuidAsync(sucursalGuid));

        [HttpPost]
        public async Task<ActionResult<SucursalDTO>> Create([FromBody] SucursalUpsertRequest request)
        {
            var created = await _service.CreateAsync(request.ToCreateDto());
            return CreatedAtAction(nameof(GetById), new { id = created.IdSucursal }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SucursalUpsertRequest request)
        {
            await _service.UpdateAsync(request.ToUpdateDto(id));
            return NoContent();
        }

        [HttpPut("{sucursalGuid:guid}")]
        public async Task<ActionResult<SucursalDTO>> UpdateByGuid(Guid sucursalGuid, [FromBody] SucursalUpsertRequest request)
        {
            var existing = await _service.GetByGuidAsync(sucursalGuid);
            await _service.UpdateAsync(request.ToUpdateDto(existing.IdSucursal));
            var updated = await _service.GetByGuidAsync(sucursalGuid);
            return Ok(updated);
        }

        [HttpPatch("{sucursalGuid:guid}/politicas")]
        public async Task<IActionResult> UpdatePoliticas(Guid sucursalGuid, [FromBody] SucursalPoliticasPatchRequest request)
        {
            var usuario = User?.Identity?.Name ?? "Sistema";
            await _service.UpdatePoliticasAsync(sucursalGuid, request.ToDto(), usuario);
            return NoContent();
        }

        [HttpPatch("{sucursalGuid:guid}/inhabilitar")]
        public async Task<IActionResult> Inhabilitar(Guid sucursalGuid, [FromBody] InhabilitarRequest request)
        {
            var usuario = User?.Identity?.Name ?? "Sistema";
            await _service.InhabilitarAsync(sucursalGuid, request.Motivo, usuario);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("{sucursalGuid:guid}")]
        public async Task<IActionResult> DeleteByGuid(Guid sucursalGuid)
        {
            await _service.DeleteAsync(sucursalGuid);
            return NoContent();
        }
    }
}
