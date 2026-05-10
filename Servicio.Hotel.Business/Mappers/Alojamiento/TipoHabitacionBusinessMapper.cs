using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.Business.Mappers.Alojamiento
{
    public static class TipoHabitacionBusinessMapper
    {
        public static TipoHabitacionDTO ToDto(this TipoHabitacionDataModel model)
        {
            if (model == null) return null;

            return new TipoHabitacionDTO
            {
                IdTipoHabitacion = model.IdTipoHabitacion,
                TipoHabitacionGuid = model.TipoHabitacionGuid,
                Slug = model.Slug,
                CodigoTipoHabitacion = model.CodigoTipoHabitacion,
                NombreTipoHabitacion = model.NombreTipoHabitacion,
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
                RowVersion = model.RowVersion,
                Imagenes = model.Imagenes.ToDtoList()
            };
        }

        public static TipoHabitacionDataModel ToDataModel(this TipoHabitacionDTO dto)
        {
            if (dto == null) return null;

            return new TipoHabitacionDataModel
            {
                IdTipoHabitacion = dto.IdTipoHabitacion,
                TipoHabitacionGuid = dto.TipoHabitacionGuid,
                Slug = dto.Slug,
                CodigoTipoHabitacion = dto.CodigoTipoHabitacion,
                NombreTipoHabitacion = dto.NombreTipoHabitacion,
                Descripcion = dto.Descripcion,
                CapacidadAdultos = dto.CapacidadAdultos,
                CapacidadNinos = dto.CapacidadNinos,
                CapacidadTotal = dto.CapacidadTotal,
                TipoCama = dto.TipoCama,
                AreaM2 = dto.AreaM2,
                PermiteEventos = dto.PermiteEventos,
                PermiteReservaPublica = dto.PermiteReservaPublica,
                EstadoTipoHabitacion = dto.EstadoTipoHabitacion,
                EsEliminado = dto.EsEliminado,
                FechaInhabilitacionUtc = dto.FechaInhabilitacionUtc,
                MotivoInhabilitacion = dto.MotivoInhabilitacion,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                ModificadoPorUsuario = dto.ModificadoPorUsuario,
                FechaModificacionUtc = dto.FechaModificacionUtc,
                ModificacionIp = dto.ModificacionIp,
                ServicioOrigen = dto.ServicioOrigen,
                RowVersion = dto.RowVersion,
                Imagenes = dto.Imagenes.ToDataModelList()
            };
        }

        public static List<TipoHabitacionDTO> ToDtoList(this IEnumerable<TipoHabitacionDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<TipoHabitacionDataModel> ToDataModelList(this IEnumerable<TipoHabitacionDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();

        public static TipoHabitacionDataModel ToDataModel(this TipoHabitacionCreateDTO dto)
        {
            if (dto == null) return null;

            return new TipoHabitacionDataModel
            {
                CodigoTipoHabitacion = dto.CodigoTipoHabitacion,
                NombreTipoHabitacion = dto.NombreTipoHabitacion,
                Descripcion = dto.Descripcion ?? string.Empty,
                CapacidadAdultos = dto.CapacidadAdultos,
                CapacidadNinos = dto.CapacidadNinos,
                CapacidadTotal = dto.CapacidadTotal,
                TipoCama = dto.TipoCama,
                AreaM2 = dto.AreaM2,
                PermiteEventos = dto.PermiteEventos,
                PermiteReservaPublica = dto.PermiteReservaPublica,
                EstadoTipoHabitacion = dto.EstadoTipoHabitacion,
                Imagenes = dto.Imagenes.ToDataModelList()
            };
        }

        public static TipoHabitacionDataModel ToDataModel(this TipoHabitacionUpdateDTO dto)
        {
            if (dto == null) return null;

            return new TipoHabitacionDataModel
            {
                IdTipoHabitacion = dto.IdTipoHabitacion,
                CodigoTipoHabitacion = dto.CodigoTipoHabitacion,
                NombreTipoHabitacion = dto.NombreTipoHabitacion,
                Descripcion = dto.Descripcion ?? string.Empty,
                CapacidadAdultos = dto.CapacidadAdultos,
                CapacidadNinos = dto.CapacidadNinos,
                CapacidadTotal = dto.CapacidadTotal,
                TipoCama = dto.TipoCama,
                AreaM2 = dto.AreaM2,
                PermiteEventos = dto.PermiteEventos,
                PermiteReservaPublica = dto.PermiteReservaPublica,
                EstadoTipoHabitacion = dto.EstadoTipoHabitacion,
                Imagenes = dto.Imagenes.ToDataModelList()
            };
        }
    }
}
