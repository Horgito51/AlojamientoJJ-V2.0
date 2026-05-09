using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Servicio.Hotel.API.Controllers.V1.Internal.Seguridad
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/internal/permisos")]
    public class PermisosController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            // Nota: En ausencia de entidades/tablas de permisos en este proyecto,
            // se expone un catálogo básico extensible.
            var permisos = new[]
            {
                "usuarios:leer",
                "usuarios:crear",
                "usuarios:actualizar",
                "usuarios:eliminar",
                "roles:leer",
                "roles:crear",
                "roles:actualizar",
                "roles:eliminar",
                "sucursales:leer",
                "sucursales:crear",
                "sucursales:actualizar",
                "sucursales:eliminar",
                "tipos-habitacion:leer",
                "tipos-habitacion:crear",
                "tipos-habitacion:actualizar",
                "tipos-habitacion:eliminar",
                "habitaciones:leer",
                "habitaciones:crear",
                "habitaciones:actualizar",
                "habitaciones:eliminar",
                "auditoria:leer"
            };

            return Ok(permisos);
        }
    }
}
