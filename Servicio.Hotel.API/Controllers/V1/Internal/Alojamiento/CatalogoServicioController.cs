using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
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
    [Route("api/v{version:apiVersion}/internal/catalogo-servicios")]
    public class CatalogoServicioController : ControllerBase
    {
        private readonly ICatalogoServicioService _service;

        public CatalogoServicioController(ICatalogoServicioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogoServicioDTO>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<CatalogoServicioDTO>> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<ActionResult<CatalogoServicioDTO>> Create([FromBody] CatalogoServicioUpsertRequest request)
        {
            var created = await _service.CreateAsync(request.ToCreateDto());
            return CreatedAtAction(nameof(GetById), new { id = created.IdCatalogo }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CatalogoServicioDTO>> Update(int id, [FromBody] CatalogoServicioUpsertRequest request)
        {
            await _service.UpdateAsync(request.ToUpdateDto(id));
            var updated = await _service.GetByIdAsync(id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
