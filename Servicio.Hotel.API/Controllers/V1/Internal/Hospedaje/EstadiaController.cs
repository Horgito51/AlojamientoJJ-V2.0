using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Servicio.Hotel.API.Models.Requests.Internal;
using Servicio.Hotel.Business.DTOs.Hospedaje;
using Servicio.Hotel.Business.Interfaces.Hospedaje;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Internal.Hospedaje
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/internal/estadias")]
    public class EstadiaController : ControllerBase
    {
        private readonly IEstadiaService _estadiaService;

        public EstadiaController(IEstadiaService estadiaService)
        {
            _estadiaService = estadiaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadiaDTO>>> GetAll()
        {
            var result = await _estadiaService.GetByFiltroAsync(new EstadiaFiltroDTO(), 1, 50);
            return Ok(result.Items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstadiaDTO>> GetById(int id)
        {
            var result = await _estadiaService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("checkin/{id_reserva}")]
        public async Task<ActionResult<IEnumerable<EstadiaDTO>>> Checkin(int id_reserva)
        {
            var result = await _estadiaService.HacerCheckinAsync(id_reserva, "Sistema");
            return StatusCode(201, result);
        }

        [HttpPatch("{id}/checkout")]
        public async Task<IActionResult> Checkout(int id, [FromBody] EstadiaCheckoutRequest request)
        {
            await _estadiaService.RegistrarCheckoutAsync(id, request.Observaciones ?? string.Empty, request.RequiereMantenimiento, "Sistema");
            return NoContent();
        }

        [HttpGet("{id}/cargos")]
        public async Task<ActionResult<IEnumerable<CargoEstadiaDTO>>> GetCargos(int id)
        {
            var result = await _estadiaService.GetCargosByEstadiaAsync(id, 1, 50);
            return Ok(result.Items);
        }

        [HttpPost("{id}/cargos")]
        public async Task<ActionResult<CargoEstadiaDTO>> AddCargo(int id, [FromBody] CargoEstadiaCreateRequest request)
        {
            var dto = request.ToDto(id);
            var result = await _estadiaService.AddCargoAsync(id, dto);
            return Ok(result);
        }
    }
}
