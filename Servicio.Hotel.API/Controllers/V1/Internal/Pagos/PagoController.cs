using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Servicio.Hotel.API.Models.Requests.Internal;
using Servicio.Hotel.Business.DTOs.Facturacion;
using Servicio.Hotel.Business.Interfaces.Facturacion;
using Servicio.Hotel.Business.Interfaces.Reservas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Internal.Pagos
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/internal/pagos")]
    public class PagoController : ControllerBase
    {
        private readonly IPagoService _pagoService;
        private readonly IReservaService _reservaService;

        public PagoController(IPagoService pagoService, IReservaService reservaService)
        {
            _pagoService = pagoService;
            _reservaService = reservaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagoDTO>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var result = await _pagoService.GetAllAsync(page, pageSize);
            return Ok(result.Items);
        }

        [HttpGet("factura/{facturaId:int}")]
        public async Task<ActionResult<IEnumerable<PagoDTO>>> GetByFactura(int facturaId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var result = await _pagoService.GetByFacturaAsync(facturaId, page, pageSize);
            return Ok(result.Items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PagoDTO>> GetById(int id)
        {
            var result = await _pagoService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<PagoDTO>> Create([FromBody] PagoCreateRequest request)
        {
            var dto = request.ToDto();
            var result = await _pagoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.IdPago }, result);
        }

        [HttpPost("/api/v{version:apiVersion}/pagos/simular")]
        public async Task<ActionResult<PagoSimuladoDTO>> Simular([FromBody] PagoSimularRequest request)
        {
            var usuario = User.Identity?.Name ?? "Cliente";
            var result = await _pagoService.SimularPagoAsync(request.IdReserva, request.Monto, usuario, request.TokenPago, request.Referencia);
            return Ok(result);
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] PagoEstadoRequest request)
        {
            await _pagoService.UpdateEstadoAsync(id, request.NuevoEstado, "Sistema");
            return NoContent();
        }
    }
}
