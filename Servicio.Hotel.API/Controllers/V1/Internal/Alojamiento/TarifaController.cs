using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Models.Requests.Internal;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Internal.Alojamiento
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/internal/tarifas")]
    public class TarifaController : ControllerBase
    {
        private readonly ITarifaService _tarifaService;

        public TarifaController(ITarifaService tarifaService)
        {
            _tarifaService = tarifaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TarifaDTO>>> GetAll()
        {
            var result = await _tarifaService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TarifaDTO>> GetById(int id)
        {
            var result = await _tarifaService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TarifaDTO>> Create([FromBody] TarifaUpsertRequest request)
        {
            var dto = request.ToCreateDto();
            var result = await _tarifaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.IdTarifa }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TarifaDTO>> Update(int id, [FromBody] TarifaUpsertRequest request)
        {
            var dto = request.ToUpdateDto(id);
            await _tarifaService.UpdateAsync(dto);
            var updated = await _tarifaService.GetByIdAsync(id);
            return Ok(updated);
        }

        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _tarifaService.DesactivarAsync(id, "Sistema");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tarifaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
