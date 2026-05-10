using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.Business.Mappers.Alojamiento
{
    public static class HabitacionBusinessMapper
    {
        public static HabitacionDTO ToDto(this HabitacionDataModel model)
        {
            if (model == null) return null;

            return new HabitacionDTO
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
                RowVersion = model.RowVersion,
                Imagenes = model.Imagenes.ToDtoList()
            };
        }

        public static HabitacionDataModel ToDataModel(this HabitacionDTO dto)
        {
            if (dto == null) return null;

            return new HabitacionDataModel
            {
                IdHabitacion = dto.IdHabitacion,
                HabitacionGuid = dto.HabitacionGuid,
                IdSucursal = dto.IdSucursal,
                IdTipoHabitacion = dto.IdTipoHabitacion,
                NumeroHabitacion = dto.NumeroHabitacion,
                Piso = dto.Piso,
                CapacidadHabitacion = dto.CapacidadHabitacion,
                PrecioBase = dto.PrecioBase,
                Url = dto.Url,
                DescripcionHabitacion = dto.DescripcionHabitacion,
                EstadoHabitacion = dto.EstadoHabitacion,
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

        public static List<HabitacionDTO> ToDtoList(this IEnumerable<HabitacionDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<HabitacionDataModel> ToDataModelList(this IEnumerable<HabitacionDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();
    }
}
