using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.DataManagement.Alojamiento.Mappers
{
    public static class HabitacionDataMapper
    {
        public static HabitacionDataModel ToModel(this HabitacionEntity entity)
        {
            if (entity == null) return null;

            return new HabitacionDataModel
            {
                IdHabitacion = entity.IdHabitacion,
                HabitacionGuid = entity.HabitacionGuid,
                IdSucursal = entity.IdSucursal,
                IdTipoHabitacion = entity.IdTipoHabitacion,
                NumeroHabitacion = entity.NumeroHabitacion,
                Piso = entity.Piso,
                CapacidadHabitacion = entity.CapacidadHabitacion,
                PrecioBase = entity.PrecioBase,
                Url = entity.Url,
                DescripcionHabitacion = entity.DescripcionHabitacion,
                EstadoHabitacion = entity.EstadoHabitacion,
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

        public static HabitacionEntity ToEntity(this HabitacionDataModel model)
        {
            if (model == null) return null;

            return new HabitacionEntity
            {
                IdHabitacion = model.IdHabitacion,
                HabitacionGuid = model.HabitacionGuid,
                IdSucursal = model.IdSucursal,
                IdTipoHabitacion = model.IdTipoHabitacion,
                NumeroHabitacion = model.NumeroHabitacion,
                Piso = model.Piso,
                CapacidadHabitacion = model.CapacidadHabitacion,
                PrecioBase = model.PrecioBase,
                Url = model.Url,
                DescripcionHabitacion = model.DescripcionHabitacion,
                EstadoHabitacion = model.EstadoHabitacion,
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

        public static List<HabitacionDataModel> ToModelList(this IEnumerable<HabitacionEntity> entities)
        {
            return entities?.Select(e => e.ToModel()).ToList() ?? new List<HabitacionDataModel>();
        }

        public static List<HabitacionEntity> ToEntityList(this IEnumerable<HabitacionDataModel> models)
        {
            return models?.Select(m => m.ToEntity()).ToList() ?? new List<HabitacionEntity>();
        }
    }
}
