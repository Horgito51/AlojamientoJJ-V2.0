using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.DataManagement.Alojamiento.Mappers
{
    public static class TarifaDataMapper
    {
        public static TarifaDataModel ToModel(this TarifaEntity entity)
        {
            if (entity == null) return null;

            return new TarifaDataModel
            {
                IdTarifa = entity.IdTarifa,
                TarifaGuid = entity.TarifaGuid,
                CodigoTarifa = entity.CodigoTarifa,
                IdSucursal = entity.IdSucursal,
                IdTipoHabitacion = entity.IdTipoHabitacion,
                NombreTarifa = entity.NombreTarifa,
                CanalTarifa = entity.CanalTarifa,
                FechaInicio = entity.FechaInicio,
                FechaFin = entity.FechaFin,
                PrecioPorNoche = entity.PrecioPorNoche,
                PorcentajeIva = entity.PorcentajeIva,
                MinNoches = entity.MinNoches,
                MaxNoches = entity.MaxNoches,
                PermitePortalPublico = entity.PermitePortalPublico,
                Prioridad = entity.Prioridad,
                EstadoTarifa = entity.EstadoTarifa,
                EsEliminado = entity.EsEliminado,
                FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
                MotivoInhabilitacion = entity.MotivoInhabilitacion,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                ServicioOrigen = entity.ServicioOrigen,
                RowVersion = entity.RowVersion
            };
        }

        public static TarifaEntity ToEntity(this TarifaDataModel model)
        {
            if (model == null) return null;

            return new TarifaEntity
            {
                IdTarifa = model.IdTarifa,
                TarifaGuid = model.TarifaGuid,
                CodigoTarifa = model.CodigoTarifa,
                IdSucursal = model.IdSucursal,
                IdTipoHabitacion = model.IdTipoHabitacion,
                NombreTarifa = model.NombreTarifa,
                CanalTarifa = model.CanalTarifa,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,
                PrecioPorNoche = model.PrecioPorNoche,
                PorcentajeIva = model.PorcentajeIva,
                MinNoches = model.MinNoches,
                MaxNoches = model.MaxNoches,
                PermitePortalPublico = model.PermitePortalPublico,
                Prioridad = model.Prioridad,
                EstadoTarifa = model.EstadoTarifa,
                EsEliminado = model.EsEliminado,
                FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
                MotivoInhabilitacion = model.MotivoInhabilitacion,
                FechaRegistroUtc = model.FechaRegistroUtc,
                CreadoPorUsuario = model.CreadoPorUsuario,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                ServicioOrigen = model.ServicioOrigen,
                RowVersion = model.RowVersion
            };
        }

        public static List<TarifaDataModel> ToModelList(this IEnumerable<TarifaEntity> entities)
            => entities?.Select(e => e.ToModel()).ToList() ?? new();

        public static List<TarifaEntity> ToEntityList(this IEnumerable<TarifaDataModel> models)
            => models?.Select(m => m.ToEntity()).ToList() ?? new();
    }
}