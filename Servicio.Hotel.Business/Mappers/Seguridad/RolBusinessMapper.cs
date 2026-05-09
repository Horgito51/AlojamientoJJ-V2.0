using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Seguridad;
using Servicio.Hotel.DataManagement.Seguridad.Models;

namespace Servicio.Hotel.Business.Mappers.Seguridad
{
    public static class RolBusinessMapper
    {
        public static RolDTO ToDto(this RolDataModel model)
        {
            if (model == null) return null;
            return new RolDTO
            {
                IdRol = model.IdRol,
                RolGuid = model.RolGuid,
                NombreRol = model.NombreRol,
                DescripcionRol = model.DescripcionRol,
                EstadoRol = model.EstadoRol,
                EsEliminado = model.EsEliminado,
                Activo = model.Activo,
                FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
                MotivoInhabilitacion = model.MotivoInhabilitacion,
                FechaRegistroUtc = model.FechaRegistroUtc,
                CreadoPorUsuario = model.CreadoPorUsuario,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                RowVersion = model.RowVersion
            };
        }

        public static RolDataModel ToDataModel(this RolDTO dto)
        {
            if (dto == null) return null;
            return new RolDataModel
            {
                IdRol = dto.IdRol,
                RolGuid = dto.RolGuid,
                NombreRol = dto.NombreRol,
                DescripcionRol = dto.DescripcionRol,
                EstadoRol = dto.EstadoRol,
                EsEliminado = dto.EsEliminado,
                Activo = dto.Activo,
                FechaInhabilitacionUtc = dto.FechaInhabilitacionUtc,
                MotivoInhabilitacion = dto.MotivoInhabilitacion,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                ModificadoPorUsuario = dto.ModificadoPorUsuario,
                FechaModificacionUtc = dto.FechaModificacionUtc,
                ModificacionIp = dto.ModificacionIp,
                RowVersion = dto.RowVersion
            };
        }

        public static List<RolDTO> ToDtoList(this IEnumerable<RolDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<RolDataModel> ToDataModelList(this IEnumerable<RolDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();
    }
}