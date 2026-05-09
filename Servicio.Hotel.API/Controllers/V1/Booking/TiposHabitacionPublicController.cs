using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Models.Responses.Public;
using Servicio.Hotel.Business.Interfaces.Alojamiento;

namespace Servicio.Hotel.API.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/public/tipos-habitacion")]
    public class TiposHabitacionPublicController : ControllerBase
    {
        private readonly ITipoHabitacionService _tipoHabitacionService;

        public TiposHabitacionPublicController(ITipoHabitacionService tipoHabitacionService)
        {
            _tipoHabitacionService = tipoHabitacionService;
        }

        [HttpGet("{tipoHabitacionGuid:guid}")]
        public async Task<ActionResult<TipoHabitacionPublicDto>> GetByGuid(Guid tipoHabitacionGuid)
        {
            var tipo = await _tipoHabitacionService.GetByGuidAsync(tipoHabitacionGuid);
            return Ok(tipo.ToPublicDto());
        }
    }
}
