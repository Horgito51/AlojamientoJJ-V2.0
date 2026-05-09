using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Models.Requests.Public;
using Servicio.Hotel.API.Models.Responses.Public;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.DTOs.Valoraciones;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using Servicio.Hotel.Business.Interfaces.Valoraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/accommodations")]
    public class AccommodationsController : ControllerBase
    {
        private readonly IHabitacionService _habitacionService;
        private readonly IValoracionService _valoracionService;
        private readonly ITipoHabitacionService _tipoHabitacionService;
        private readonly ISucursalService _sucursalService;

        public AccommodationsController(
            IHabitacionService habitacionService,
            IValoracionService valoracionService,
            ITipoHabitacionService tipoHabitacionService,
            ISucursalService sucursalService)
        {
            _habitacionService = habitacionService;
            _valoracionService = valoracionService;
            _tipoHabitacionService = tipoHabitacionService;
            _sucursalService = sucursalService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<HabitacionPublicDto>>> Search([FromQuery] string query)
        {
            RejectIdQueryParameters();
            var habitaciones = await _habitacionService.GetAllAsync();
            var sucursales = await _sucursalService.GetAllAsync();
            var tipos = await _tipoHabitacionService.GetAllAsync();
            var normalizedQuery = (query ?? string.Empty).Trim().ToLowerInvariant();

            var result = habitaciones
                .Where(h => string.IsNullOrWhiteSpace(normalizedQuery)
                    || h.NumeroHabitacion.ToLowerInvariant().Contains(normalizedQuery)
                    || (h.DescripcionHabitacion ?? string.Empty).ToLowerInvariant().Contains(normalizedQuery))
                .Select(h =>
                {
                    var sucursal = sucursales.First(s => s.IdSucursal == h.IdSucursal);
                    var tipo = tipos.First(t => t.IdTipoHabitacion == h.IdTipoHabitacion);
                    return h.ToPublicDto(sucursal, tipo);
                });

            return Ok(result);
        }

        private void RejectIdQueryParameters()
        {
            foreach (var key in Request.Query.Keys)
            {
                if (PublicRequestGuard.IsIdProperty(key))
                    throw new ValidationException("PUB-GUID-QUERY-001", $"El parametro '{key}' no esta permitido en endpoints publicos. Use GUIDs.");
            }
        }

        [HttpGet("{habitacionGuid:guid}")]
        public async Task<ActionResult<HabitacionPublicDto>> GetByGuid(Guid habitacionGuid)
        {
            var habitacion = await _habitacionService.GetByGuidAsync(habitacionGuid);
            var sucursal = await _sucursalService.GetByIdAsync(habitacion.IdSucursal);
            var tipo = await _tipoHabitacionService.GetByIdAsync(habitacion.IdTipoHabitacion);
            return Ok(habitacion.ToPublicDto(sucursal, tipo));
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<TipoHabitacionPublicDto>>> GetCategories()
        {
            var result = await _tipoHabitacionService.GetAllAsync();
            return Ok(result.Select(t => t.ToPublicDto()));
        }

        [HttpGet("{habitacionGuid:guid}/reviews")]
        public async Task<ActionResult<IEnumerable<ValoracionPublicDto>>> GetReviews(Guid habitacionGuid)
        {
            var habitacion = await _habitacionService.GetByGuidAsync(habitacionGuid);
            var filtro = new ValoracionFiltroDTO { IdHabitacion = habitacion.IdHabitacion };
            var result = await _valoracionService.GetByFiltroAsync(filtro, 1, 10);
            return Ok(result.Items.Select(v => v.ToPublicDto()));
        }
    }
}
