using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicio.Hotel.API.Models.Common;
using Servicio.Hotel.DataAccess.Context;

namespace Servicio.Hotel.API.Controllers.V1.Internal.Seguridad
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/internal/auditoria")]
    public class AuditoriaController : ControllerBase
    {
        private readonly ServicioHotelDbContext _db;

        public AuditoriaController(ServicioHotelDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? tabla = null)
        {
            var query = _db.Auditorias.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(tabla))
                query = query.Where(a => a.TablaAfectada == tabla);

            var items = await query
                .OrderByDescending(a => a.FechaEventoUtc)
                .Select(a => new
                {
                    auditoriaGuid = a.AuditoriaGuid,
                    tablaAfectada = a.TablaAfectada,
                    operacion = a.Operacion,
                    idRegistroAfectado = a.IdRegistroAfectado,
                    usuarioEjecutor = a.UsuarioEjecutor,
                    ipOrigen = a.IpOrigen,
                    servicioOrigen = a.ServicioOrigen,
                    fechaEventoUtc = a.FechaEventoUtc
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{auditoriaGuid:guid}")]
        public async Task<IActionResult> GetByGuid(Guid auditoriaGuid)
        {
            var item = await _db.Auditorias.AsNoTracking()
                .Where(a => a.AuditoriaGuid == auditoriaGuid)
                .Select(a => new
                {
                    auditoriaGuid = a.AuditoriaGuid,
                    tablaAfectada = a.TablaAfectada,
                    operacion = a.Operacion,
                    idRegistroAfectado = a.IdRegistroAfectado,
                    datosAnteriores = a.DatosAnteriores,
                    datosNuevos = a.DatosNuevos,
                    usuarioEjecutor = a.UsuarioEjecutor,
                    ipOrigen = a.IpOrigen,
                    servicioOrigen = a.ServicioOrigen,
                    fechaEventoUtc = a.FechaEventoUtc
                })
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound(ApiErrorResponse.NotFound("Auditoría no encontrada.", HttpContext.TraceIdentifier));

            return Ok(item);
        }
    }
}
