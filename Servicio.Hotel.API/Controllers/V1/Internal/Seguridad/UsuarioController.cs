using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Servicio.Hotel.API.Models.Requests.Internal;
using Servicio.Hotel.API.Models.Common;
using Servicio.Hotel.Business.DTOs.Seguridad;
using Servicio.Hotel.Business.Interfaces.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Hotel.API.Controllers.V1.Internal.Seguridad
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/internal/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
 
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50, [FromQuery] string? estado = null)
        {
            if (!string.IsNullOrWhiteSpace(estado) && estado != "ACT" && estado != "INA" && estado != "BLO")
                return BadRequest(ApiErrorResponse.BadRequest("El parámetro estado es inválido. Use: ACT, INA o BLO.", null, HttpContext.TraceIdentifier));

            var pagedResult = await _usuarioService.GetAllPagedAsync(page, pageSize);

            var items = pagedResult.Items;
            if (!string.IsNullOrWhiteSpace(estado))
                items = items.Where(u => u.EstadoUsuario == estado).ToList();
            else
                items = items.Where(u => u.EstadoUsuario != "INA" && u.EstadoUsuario != "BLO").ToList();

            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuarioDTO>> GetById(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            return Ok(usuario);
        }

        [HttpGet("{usuarioGuid:guid}")]
        public async Task<ActionResult<UsuarioDTO>> GetByGuid(Guid usuarioGuid)
        {
            var usuario = await _usuarioService.GetByGuidAsync(usuarioGuid);
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> Create([FromBody] UsuarioCreateRequest request)
        {
            var usuarioCreateDto = request.ToCreateDto();
            var nuevoUsuario = await _usuarioService.CreateAsync(usuarioCreateDto);
            return CreatedAtAction(nameof(GetById), new { id = nuevoUsuario.IdUsuario }, nuevoUsuario);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpdateRequest request)
        {
            var usuarioUpdateDto = request.ToUpdateDto(id);
            await _usuarioService.UpdateAsync(usuarioUpdateDto);
            return NoContent();
        }

        [HttpPut("{usuarioGuid:guid}")]
        public async Task<ActionResult<UsuarioDTO>> UpdateByGuid(Guid usuarioGuid, [FromBody] UsuarioUpdateRequest request)
        {
            var existing = await _usuarioService.GetByGuidAsync(usuarioGuid);
            var usuarioUpdateDto = request.ToUpdateDto(existing.IdUsuario);
            await _usuarioService.UpdateAsync(usuarioUpdateDto);
            var updated = await _usuarioService.GetByGuidAsync(usuarioGuid);
            return Ok(updated);
        }

        [HttpPatch("{id:int}/inhabilitar")]
        public async Task<IActionResult> Inhabilitar(int id, [FromBody] InhabilitarRequest request)
        {
            await _usuarioService.InhabilitarAsync(id, request.Motivo, "Sistema");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _usuarioService.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("{usuarioGuid:guid}")]
        public async Task<IActionResult> DeleteByGuid(Guid usuarioGuid)
        {
            var existing = await _usuarioService.GetByGuidAsync(usuarioGuid);
            await _usuarioService.DeleteAsync(existing.IdUsuario);
            return NoContent();
        }
    }
}
