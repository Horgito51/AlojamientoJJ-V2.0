using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/v{version:apiVersion}/internal/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var pagedResult = await _clienteService.GetAllPagedAsync(page, pageSize);
            return Ok(pagedResult.Items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetById(int id)
        {
            var result = await _clienteService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> Create([FromBody] ClienteCreateRequest request)
        {
            var dto = request.ToCreateDto();
            var result = await _clienteService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.IdCliente }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateRequest request)
        {
            var dto = request.ToUpdateDto(id);
            await _clienteService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clienteService.DeleteAsync(id);
            return NoContent();
        }
    }
}
