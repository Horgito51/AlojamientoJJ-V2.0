using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.DataManagement.Alojamiento.Mappers
{
    public static class TipoHabitacionDataMapper
    {
        public static TipoHabitacionDataModel ToModel(this TipoHabitacionEntity entity)
        {
            if (entity == null) return null;

            return new TipoHabitacionDataModel
            {
                IdTipoHabitacion = entity.IdTipoHabitacion,
                TipoHabitacionGuid = entity.TipoHabitacionGuid,
                CodigoTipoHabitacion = entity.CodigoTipoHabitacion,
                NombreTipoHabitacion = entity.NombreTipoHabitacion,
                Slug = entity.Slug,
                Descripcion = entity.Descripcion,
                CapacidadAdultos = entity.CapacidadAdultos,
                CapacidadNinos = entity.CapacidadNinos,
                CapacidadTotal = entity.CapacidadTotal,
                TipoCama = entity.TipoCama,
                AreaM2 = entity.AreaM2,
                PermiteEventos = entity.PermiteEventos,
                PermiteReservaPublica = entity.PermiteReservaPublica,
                EstadoTipoHabitacion = entity.EstadoTipoHabitacion,
                EsEliminado = entity.EsEliminado,
                FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
                MotivoInhabilitacion = entity.MotivoInhabilitacion,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                ServicioOrigen = entity.ServicioOrigen,
                RowVersion = entity.RowVersion,
                Imagenes = entity.TipoHabitacionImagenes.ToTipoImagenModelList()
            };
        }

        public static TipoHabitacionEntity ToEntity(this TipoHabitacionDataModel model)
        {
            if (model == null) return null;

            return new TipoHabitacionEntity
            {
                IdTipoHabitacion = model.IdTipoHabitacion,
                TipoHabitacionGuid = model.TipoHabitacionGuid,
                CodigoTipoHabitacion = model.CodigoTipoHabitacion,
                NombreTipoHabitacion = model.NombreTipoHabitacion,
                Slug = model.Slug,
                Descripcion = model.Descripcion,
                CapacidadAdultos = model.CapacidadAdultos,
                CapacidadNinos = model.CapacidadNinos,
                CapacidadTotal = model.CapacidadTotal,
                TipoCama = model.TipoCama,
                AreaM2 = model.AreaM2,
                PermiteEventos = model.PermiteEventos,
                PermiteReservaPublica = model.PermiteReservaPublica,
                EstadoTipoHabitacion = model.EstadoTipoHabitacion,
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

        public static List<TipoHabitacionDataModel> ToModelList(this IEnumerable<TipoHabitacionEntity> entities)
            => entities?.Select(e => e.ToModel()).ToList() ?? new();

        public static List<TipoHabitacionEntity> ToEntityList(this IEnumerable<TipoHabitacionDataModel> models)
            => models?.Select(m => m.ToEntity()).ToList() ?? new();
    }
}
