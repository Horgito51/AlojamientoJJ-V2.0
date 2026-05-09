using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Valoraciones;
using Servicio.Hotel.DataManagement.Valoraciones.Models;

namespace Servicio.Hotel.Business.Mappers.Valoraciones
{
    public static class ValoracionBusinessMapper
    {
        public static ValoracionDTO ToDto(this ValoracionDataModel model)
        {
            if (model == null) return null;

            return new ValoracionDTO
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

        public static ValoracionDataModel ToDataModel(this ValoracionDTO dto)
        {
            if (dto == null) return null;

            return new ValoracionDataModel
            {
                IdValoracion = dto.IdValoracion,
                ValoracionGuid = dto.ValoracionGuid,
                IdEstadia = dto.IdEstadia,
                IdCliente = dto.IdCliente,
                IdSucursal = dto.IdSucursal,
                IdHabitacion = dto.IdHabitacion,
                PuntuacionGeneral = dto.PuntuacionGeneral,
                PuntuacionLimpieza = dto.PuntuacionLimpieza,
                PuntuacionConfort = dto.PuntuacionConfort,
                PuntuacionUbicacion = dto.PuntuacionUbicacion,
                PuntuacionInstalaciones = dto.PuntuacionInstalaciones,
                PuntuacionPersonal = dto.PuntuacionPersonal,
                PuntuacionCalidadPrecio = dto.PuntuacionCalidadPrecio,
                ComentarioPositivo = dto.ComentarioPositivo,
                ComentarioNegativo = dto.ComentarioNegativo,
                TipoViaje = dto.TipoViaje,
                EstadoValoracion = dto.EstadoValoracion,
                PublicadaEnPortal = dto.PublicadaEnPortal,
                RespuestaHotel = dto.RespuestaHotel,
                FechaRespuestaUtc = dto.FechaRespuestaUtc,
                ModeradaPorUsuario = dto.ModeradaPorUsuario,
                MotivoModeracion = dto.MotivoModeracion,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                ModificadoPorUsuario = dto.ModificadoPorUsuario,
                FechaModificacionUtc = dto.FechaModificacionUtc,
                ModificacionIp = dto.ModificacionIp,
                ServicioOrigen = dto.ServicioOrigen,
                RowVersion = dto.RowVersion
            };
        }

        public static List<ValoracionDTO> ToDtoList(this IEnumerable<ValoracionDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<ValoracionDataModel> ToDataModelList(this IEnumerable<ValoracionDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();

        public static ValoracionFiltroDataModel ToDataModel(this ValoracionFiltroDTO dto)
        {
            if (dto == null) return null;

            return new ValoracionFiltroDataModel
            {
                IdSucursal = dto.IdSucursal,
                IdCliente = dto.IdCliente,
                EstadoValoracion = dto.EstadoValoracion,
                TipoViaje = dto.TipoViaje,
                PublicadaEnPortal = dto.PublicadaEnPortal,
                PuntuacionMin = dto.PuntuacionMin,
                PuntuacionMax = dto.PuntuacionMax,
                FechaDesde = dto.FechaDesde,
                FechaHasta = dto.FechaHasta
            };
        }
    }
}