using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.Business.Mappers.Alojamiento
{
    public static class TarifaBusinessMapper
    {
        public static TarifaDTO ToDto(this TarifaDataModel model)
        {
            if (model == null) return null;

            return new TarifaDTO
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

        public static TarifaDataModel ToDataModel(this TarifaDTO dto)
        {
            if (dto == null) return null;

            return new TarifaDataModel
            {
                IdTarifa = dto.IdTarifa,
                TarifaGuid = dto.TarifaGuid,
                CodigoTarifa = dto.CodigoTarifa,
                IdSucursal = dto.IdSucursal,
                IdTipoHabitacion = dto.IdTipoHabitacion,
                NombreTarifa = dto.NombreTarifa,
                CanalTarifa = dto.CanalTarifa,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                PrecioPorNoche = dto.PrecioPorNoche,
                PorcentajeIva = dto.PorcentajeIva,
                MinNoches = dto.MinNoches,
                MaxNoches = dto.MaxNoches,
                PermitePortalPublico = dto.PermitePortalPublico,
                Prioridad = dto.Prioridad,
                EstadoTarifa = dto.EstadoTarifa,
                EsEliminado = dto.EsEliminado,
                FechaInhabilitacionUtc = dto.FechaInhabilitacionUtc,
                MotivoInhabilitacion = dto.MotivoInhabilitacion,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                ModificadoPorUsuario = dto.ModificadoPorUsuario,
                FechaModificacionUtc = dto.FechaModificacionUtc,
                ModificacionIp = dto.ModificacionIp,
                ServicioOrigen = dto.ServicioOrigen,
                RowVersion = dto.RowVersion
            };
        }

        public static List<TarifaDTO> ToDtoList(this IEnumerable<TarifaDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<TarifaDataModel> ToDataModelList(this IEnumerable<TarifaDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();

        public static TarifaDataModel ToDataModel(this TarifaCreateDTO dto)
        {
            if (dto == null) return null;

            return new TarifaDataModel
            {
                CodigoTarifa = dto.CodigoTarifa,
                IdSucursal = dto.IdSucursal,
                IdTipoHabitacion = dto.IdTipoHabitacion,
                NombreTarifa = dto.NombreTarifa,
                CanalTarifa = dto.CanalTarifa,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                PrecioPorNoche = dto.PrecioPorNoche,
                PorcentajeIva = dto.PorcentajeIva,
                MinNoches = dto.MinNoches,
                MaxNoches = dto.MaxNoches,
                PermitePortalPublico = dto.PermitePortalPublico,
                Prioridad = dto.Prioridad,
                EstadoTarifa = dto.EstadoTarifa
            };
        }

        public static TarifaDataModel ToDataModel(this TarifaUpdateDTO dto)
        {
            if (dto == null) return null;

            return new TarifaDataModel
            {
                IdTarifa = dto.IdTarifa,
                CodigoTarifa = dto.CodigoTarifa,
                IdSucursal = dto.IdSucursal,
                IdTipoHabitacion = dto.IdTipoHabitacion,
                NombreTarifa = dto.NombreTarifa,
                CanalTarifa = dto.CanalTarifa,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                PrecioPorNoche = dto.PrecioPorNoche,
                PorcentajeIva = dto.PorcentajeIva,
                MinNoches = dto.MinNoches,
                MaxNoches = dto.MaxNoches,
                PermitePortalPublico = dto.PermitePortalPublico,
                Prioridad = dto.Prioridad,
                EstadoTarifa = dto.EstadoTarifa
            };
        }
    }
}