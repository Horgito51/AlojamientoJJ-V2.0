using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Models.Requests.Public;
using Servicio.Hotel.API.Models.Responses.Public;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Alojamiento;

namespace Servicio.Hotel.API.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/public/habitaciones")]
    public class HabitacionesPublicController : ControllerBase
    {
        private readonly IHabitacionService _habitacionService;
        private readonly ISucursalService _sucursalService;
        private readonly ITipoHabitacionService _tipoHabitacionService;

        public HabitacionesPublicController(
            IHabitacionService habitacionService, 
            ISucursalService sucursalService, 
            ITipoHabitacionService tipoHabitacionService)
        {
            _habitacionService = habitacionService;
            _sucursalService = sucursalService;
            _tipoHabitacionService = tipoHabitacionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HabitacionPublicDto>>> GetAll(
            [FromQuery] DateTime? fechaInicio = null, 
            [FromQuery] DateTime? fechaFin = null,
            [FromQuery] Guid? sucursalGuid = null)
        {
            RejectIdQueryParameters();
            IEnumerable<HabitacionDTO> habitaciones;
            var sucursales = await _sucursalService.GetAllAsync();

            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                if (fechaFin.Value <= fechaInicio.Value)
                    throw new ValidationException("HAB-PUB-001", "La fecha de fin debe ser posterior a la fecha de inicio.");

                if (sucursalGuid.HasValue)
                {
                    var sucursal = await _sucursalService.GetByGuidAsync(sucursalGuid.Value);
                    habitaciones = await _habitacionService.GetDisponiblesAsync(sucursal.IdSucursal, fechaInicio.Value, fechaFin.Value);
                }
                else
                {
                    var disponibles = new List<HabitacionDTO>();
                    foreach (var sucursal in sucursales)
                    {
                        disponibles.AddRange(await _habitacionService.GetDisponiblesAsync(sucursal.IdSucursal, fechaInicio.Value, fechaFin.Value));
                    }
                    habitaciones = disponibles;
                }
            }
            else
            {
                habitaciones = sucursalGuid.HasValue
                    ? await _habitacionService.GetBySucursalAsync((await _sucursalService.GetByGuidAsync(sucursalGuid.Value)).IdSucursal)
                    : await _habitacionService.GetAllAsync();
            }

            var tipos = await _tipoHabitacionService.GetAllAsync();
            
            var result = habitaciones
                .Where(h => h.EstadoHabitacion == "DIS") // Solo habitaciones disponibles
                .Select(h => {
                    var sucursal = sucursales.FirstOrDefault(s => s.IdSucursal == h.IdSucursal);
                    var tipo = tipos.FirstOrDefault(t => t.IdTipoHabitacion == h.IdTipoHabitacion);
                    
                    return new HabitacionPublicDto
                    {
                        HabitacionGuid = h.HabitacionGuid,
                        NumeroHabitacion = h.NumeroHabitacion,
                        Piso = h.Piso,
                        CapacidadHabitacion = h.CapacidadHabitacion,
                        PrecioBase = h.PrecioBase, // Ya viene calculado por HabitacionService
                        DescripcionHabitacion = h.DescripcionHabitacion,
                        EstadoHabitacion = h.EstadoHabitacion,
                        SucursalGuid = sucursal?.SucursalGuid ?? Guid.Empty,
                        TipoHabitacionGuid = tipo?.TipoHabitacionGuid ?? Guid.Empty,
                        TipoHabitacionSlug = tipo?.Slug ?? string.Empty,
                        ImagenUrl = h.Url
                    };
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
            
            var dto = new HabitacionPublicDto
            {
                HabitacionGuid = habitacion.HabitacionGuid,
                NumeroHabitacion = habitacion.NumeroHabitacion,
                Piso = habitacion.Piso,
                CapacidadHabitacion = habitacion.CapacidadHabitacion,
                PrecioBase = habitacion.PrecioBase, // Ya viene calculado por HabitacionService
                DescripcionHabitacion = habitacion.DescripcionHabitacion,
                EstadoHabitacion = habitacion.EstadoHabitacion,
                SucursalGuid = sucursal.SucursalGuid,
                TipoHabitacionGuid = tipo.TipoHabitacionGuid,
                TipoHabitacionSlug = tipo.Slug,
                ImagenUrl = habitacion.Url
            };

            return Ok(dto);
        }
    }
}
