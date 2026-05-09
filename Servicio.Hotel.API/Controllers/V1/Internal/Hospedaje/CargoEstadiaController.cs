using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Servicio.Hotel.Business.Interfaces.Hospedaje;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Internal.Hospedaje
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/internal/cargos-estadia")]
    public class CargoEstadiaController : ControllerBase
    {
        private readonly IEstadiaService _estadiaService;

        public CargoEstadiaController(IEstadiaService estadiaService)
        {
            _estadiaService = estadiaService;
        }

        [HttpPatch("{id}/anular")]
        public async Task<IActionResult> Anular(int id)
        {
            await _estadiaService.AnularCargoAsync(id, "Sistema");
            return NoContent();
        }
    }
}
