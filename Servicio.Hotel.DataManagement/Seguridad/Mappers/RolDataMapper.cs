using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Seguridad;
using Servicio.Hotel.DataManagement.Seguridad.Models;

namespace Servicio.Hotel.DataManagement.Seguridad.Mappers
{
    public static class RolDataMapper
    {
        public static RolDataModel? ToModel(this RolEntity? entity)
        {
            if (entity == null) return null;

            return new RolDataModel
            {
                IdRol = entity.IdRol,
                RolGuid = entity.RolGuid,
                NombreRol = entity.NombreRol,
                DescripcionRol = entity.DescripcionRol,
                EstadoRol = entity.EstadoRol,
                EsEliminado = entity.EsEliminado,
                Activo = entity.Activo,
                FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
                MotivoInhabilitacion = entity.MotivoInhabilitacion,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                RowVersion = entity.RowVersion
            };
        }

        public static RolEntity? ToEntity(this RolDataModel? model)
        {
            if (model == null) return null;

            return new RolEntity
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

        public static List<RolDataModel> ToModelList(this IEnumerable<RolEntity> entities)
            => entities?.Select(e => e.ToModel()).ToList() ?? new();

        public static List<RolEntity> ToEntityList(this IEnumerable<RolDataModel> models)
            => models?.Select(m => m.ToEntity()).ToList() ?? new();
    }
}