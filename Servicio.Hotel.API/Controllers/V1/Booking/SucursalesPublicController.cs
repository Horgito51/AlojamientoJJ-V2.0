using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Models.Responses.Public;
using Servicio.Hotel.Business.Interfaces.Alojamiento;

namespace Servicio.Hotel.API.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/public/sucursales")]
    public class SucursalesPublicController : ControllerBase
    {
        private readonly ISucursalService _sucursalService;

        public SucursalesPublicController(ISucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }

        [HttpGet("{sucursalGuid:guid}")]
        public async Task<ActionResult<SucursalPublicDto>> GetByGuid(Guid sucursalGuid)
        {
            var sucursal = await _sucursalService.GetByGuidAsync(sucursalGuid);
            return Ok(sucursal.ToPublicDto());
        }
    }
}
