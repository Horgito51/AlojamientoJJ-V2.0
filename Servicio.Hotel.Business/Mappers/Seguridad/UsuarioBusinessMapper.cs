using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Seguridad;
using Servicio.Hotel.DataManagement.Seguridad.Models;

namespace Servicio.Hotel.Business.Mappers.Seguridad
{
    public static class UsuarioBusinessMapper
    {
        public static UsuarioDTO ToDto(this UsuarioDataModel model)
        {
            if (model == null) return null;

            return new UsuarioDTO
            {
                IdUsuario = model.IdUsuario,
                UsuarioGuid = model.UsuarioGuid,
                IdCliente = model.IdCliente,
                Username = model.Username,
                Correo = model.Correo,
                Nombres = model.Nombres,
                Apellidos = model.Apellidos,
                EstadoUsuario = model.EstadoUsuario,
                EsEliminado = model.EsEliminado,
                Activo = model.Activo,
                FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
                MotivoInhabilitacion = model.MotivoInhabilitacion,
                FechaRegistroUtc = model.FechaRegistroUtc,
                CreadoPorUsuario = model.CreadoPorUsuario,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                RowVersion = model.RowVersion,
                Roles = model.Roles?.Select(r => r.ToDto()).ToList() // ✅ Usa el ToDto de RolBusinessMapper
            };
        }

        public static UsuarioDataModel ToDataModel(this UsuarioDTO dto)
        {
            if (dto == null) return null;

            return new UsuarioDataModel
            {
                IdUsuario = dto.IdUsuario,
                UsuarioGuid = dto.UsuarioGuid,
                IdCliente = dto.IdCliente,
                Username = dto.Username,
                Correo = dto.Correo,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                EstadoUsuario = dto.EstadoUsuario,
                EsEliminado = dto.EsEliminado,
                Activo = dto.Activo,
                FechaInhabilitacionUtc = dto.FechaInhabilitacionUtc,
                MotivoInhabilitacion = dto.MotivoInhabilitacion,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                ModificadoPorUsuario = dto.ModificadoPorUsuario,
                FechaModificacionUtc = dto.FechaModificacionUtc,
                ModificacionIp = dto.ModificacionIp,
                RowVersion = dto.RowVersion,
                Roles = dto.Roles?.Select(r => r.ToDataModel()).ToList()
            };
        }

        public static List<UsuarioDTO> ToDtoList(this IEnumerable<UsuarioDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<UsuarioDataModel> ToDataModelList(this IEnumerable<UsuarioDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();
    }
}