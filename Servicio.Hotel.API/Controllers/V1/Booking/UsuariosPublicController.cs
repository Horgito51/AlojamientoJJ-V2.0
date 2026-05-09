using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Models.Responses.Public;
using Servicio.Hotel.Business.Interfaces.Seguridad;

namespace Servicio.Hotel.API.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/public/usuarios")]
    public class UsuariosPublicController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosPublicController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("{usuarioGuid:guid}")]
        public async Task<ActionResult<UsuarioPublicDto>> GetByGuid(Guid usuarioGuid)
        {
            var usuario = await _usuarioService.GetByGuidAsync(usuarioGuid);
            return Ok(usuario.ToPublicDto());
        }
    }
}
