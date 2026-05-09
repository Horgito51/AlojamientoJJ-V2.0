using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Valoraciones;
using Servicio.Hotel.DataManagement.Valoraciones.Models;

namespace Servicio.Hotel.DataManagement.Valoraciones.Mappers
{
    public static class ValoracionDataMapper
    {
        public static ValoracionDataModel ToModel(this ValoracionEntity entity)
        {
            if (entity == null) return null;

            return new ValoracionDataModel
            {
                IdValoracion = entity.IdValoracion,
                ValoracionGuid = entity.ValoracionGuid,
                IdEstadia = entity.IdEstadia,
                IdCliente = entity.IdCliente,
                IdSucursal = entity.IdSucursal,
                IdHabitacion = entity.IdHabitacion,
                PuntuacionGeneral = entity.PuntuacionGeneral,
                PuntuacionLimpieza = entity.PuntuacionLimpieza,
                PuntuacionConfort = entity.PuntuacionConfort,
                PuntuacionUbicacion = entity.PuntuacionUbicacion,
                PuntuacionInstalaciones = entity.PuntuacionInstalaciones,
                PuntuacionPersonal = entity.PuntuacionPersonal,
                PuntuacionCalidadPrecio = entity.PuntuacionCalidadPrecio,
                ComentarioPositivo = entity.ComentarioPositivo,
                ComentarioNegativo = entity.ComentarioNegativo,
                TipoViaje = entity.TipoViaje,
                EstadoValoracion = entity.EstadoValoracion,
                PublicadaEnPortal = entity.PublicadaEnPortal,
                RespuestaHotel = entity.RespuestaHotel,
                FechaRespuestaUtc = entity.FechaRespuestaUtc,
                ModeradaPorUsuario = entity.ModeradaPorUsuario,
                MotivoModeracion = entity.MotivoModeracion,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                ServicioOrigen = entity.ServicioOrigen,
                RowVersion = entity.RowVersion
            };
        }

        public static ValoracionEntity ToEntity(this ValoracionDataModel model)
        {
            if (model == null) return null;

            return new ValoracionEntity
            {
                IdValoracion = model.IdValoracion,
                ValoracionGuid = model.ValoracionGuid,
                IdEstadia = model.IdEstadia,
                IdCliente = model.IdCliente,
                IdSucursal = model.IdSucursal,
                IdHabitacion = model.IdHabitacion,
                PuntuacionGeneral = model.PuntuacionGeneral,
                PuntuacionLimpieza = model.PuntuacionLimpieza,
                PuntuacionConfort = model.PuntuacionConfort,
                PuntuacionUbicacion = model.PuntuacionUbicacion,
                PuntuacionInstalaciones = model.PuntuacionInstalaciones,
                PuntuacionPersonal = model.PuntuacionPersonal,
                PuntuacionCalidadPrecio = model.PuntuacionCalidadPrecio,
                ComentarioPositivo = model.ComentarioPositivo,
                ComentarioNegativo = model.ComentarioNegativo,
                TipoViaje = model.TipoViaje,
                EstadoValoracion = model.EstadoValoracion,
                PublicadaEnPortal = model.PublicadaEnPortal,
                RespuestaHotel = model.RespuestaHotel,
                FechaRespuestaUtc = model.FechaRespuestaUtc,
                ModeradaPorUsuario = model.ModeradaPorUsuario,
                MotivoModeracion = model.MotivoModeracion,
                FechaRegistroUtc = model.FechaRegistroUtc,
                CreadoPorUsuario = model.CreadoPorUsuario,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                ServicioOrigen = model.ServicioOrigen,
                RowVersion = model.RowVersion
            };
        }

        public static List<ValoracionDataModel> ToModelList(this IEnumerable<ValoracionEntity> entities)
            => entities?.Select(e => e.ToModel()).ToList() ?? new();

        public static List<ValoracionEntity> ToEntityList(this IEnumerable<ValoracionDataModel> models)
            => models?.Select(m => m.ToEntity()).ToList() ?? new();
    }
}