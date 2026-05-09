using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Models.Responses.Public;
using Servicio.Hotel.Business.Interfaces.Seguridad;

namespace Servicio.Hotel.API.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/public/roles")]
    public class RolesPublicController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolesPublicController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet("{rolGuid:guid}")]
        public async Task<ActionResult<RolPublicDto>> GetByGuid(Guid rolGuid)
        {
            var rol = await _rolService.GetByGuidAsync(rolGuid);
            return Ok(rol.ToPublicDto());
        }
    }
}
