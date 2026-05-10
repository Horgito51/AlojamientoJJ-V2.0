using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Models.Requests.Internal;
using Servicio.Hotel.API.Models.Common;
using Servicio.Hotel.API.Models.Responses.Internal;
using Servicio.Hotel.API.Models.Responses.Public;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Internal.Alojamiento
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/internal/habitaciones")]
    public class HabitacionController : ControllerBase
    {
        private readonly IHabitacionService _habitacionService;
        private readonly ITipoHabitacionService _tipoHabitacionService;
        private readonly ISucursalService _sucursalService;

        public HabitacionController(IHabitacionService habitacionService, ITipoHabitacionService tipoHabitacionService, ISucursalService sucursalService)
        {
            _habitacionService = habitacionService;
            _tipoHabitacionService = tipoHabitacionService;
            _sucursalService = sucursalService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HabitacionDTO>>> GetAll([FromQuery] string? estado = null)
        {
            if (!string.IsNullOrWhiteSpace(estado) && estado != "DIS" && estado != "OCU" && estado != "MNT" && estado != "FDS" && estado != "INA")
                return BadRequest(ApiErrorResponse.BadRequest("El parámetro estado es inválido. Use: DIS, OCU, MNT, FDS o INA.", null, HttpContext.TraceIdentifier));

            var result = await _habitacionService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(estado))
                return Ok(result.Where(h => h.EstadoHabitacion == estado));

            return Ok(result.Where(h => h.EstadoHabitacion != "INA"));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<HabitacionDTO>> GetById(int id)
        {
            var result = await _habitacionService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("{habitacionGuid:guid}")]
        public async Task<ActionResult<HabitacionDetalleResponse>> GetByGuid(Guid habitacionGuid)
        {
            var habitacion = await _habitacionService.GetByGuidAsync(habitacionGuid);
            var tipo = await _tipoHabitacionService.GetByIdAsync(habitacion.IdTipoHabitacion);
            var sucursal = await _sucursalService.GetByIdAsync(habitacion.IdSucursal);

            var response = new HabitacionDetalleResponse
            {
                HabitacionGuid = habitacion.HabitacionGuid,
                NumeroHabitacion = habitacion.NumeroHabitacion,
                Piso = habitacion.Piso,
                CapacidadHabitacion = habitacion.CapacidadHabitacion,
                PrecioBase = habitacion.PrecioBase,
                ImagenUrl = habitacion.Url,
                DescripcionHabitacion = habitacion.DescripcionHabitacion,
                EstadoHabitacion = habitacion.EstadoHabitacion,
                SucursalGuid = sucursal.SucursalGuid,
                Imagenes = habitacion.Imagenes.Select(i => i.ToPublicDto()).ToList(),
                TipoHabitacion = new TipoHabitacionRef
                {
                    TipoHabitacionGuid = tipo.TipoHabitacionGuid,
                    Slug = tipo.Slug,
                    NombreTipoHabitacion = tipo.NombreTipoHabitacion
                }
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<HabitacionDTO>> Create([FromBody] HabitacionCreateRequest request)
        {
            var dto = request.ToCreateDto();
            var result = await _habitacionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.IdHabitacion }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] HabitacionUpdateRequest request)
        {
            var dto = request.ToUpdateDto(id);
            await _habitacionService.UpdateAsync(dto);
            var updated = await _habitacionService.GetByIdAsync(id);
            return Ok(updated);
        }

        [HttpPatch("{id:int}/estado")]
        public async Task<ActionResult<HabitacionDTO>> ChangeStatus(int id, [FromBody] HabitacionEstadoRequest request)
        {
            await _habitacionService.UpdateEstadoAsync(id, request.NuevoEstado, "Sistema");
            var updated = await _habitacionService.GetByIdAsync(id);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _habitacionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
