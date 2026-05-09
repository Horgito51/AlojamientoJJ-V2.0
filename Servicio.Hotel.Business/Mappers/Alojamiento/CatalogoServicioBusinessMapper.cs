using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.Business.Mappers.Alojamiento
{
    public static class CatalogoServicioBusinessMapper
    {
        public static CatalogoServicioDTO? ToDto(this CatalogoServicioDataModel? model)
        {
            if (model == null) return null;

            return new CatalogoServicioDTO
            {
                IdCatalogo = model.IdCatalogo,
                CatalogoGuid = model.CatalogoGuid,
                IdSucursal = model.IdSucursal,
                CodigoCatalogo = model.CodigoCatalogo,
                NombreCatalogo = model.NombreCatalogo,
                TipoCatalogo = model.TipoCatalogo,
                CategoriaCatalogo = model.CategoriaCatalogo,
                DescripcionCatalogo = model.DescripcionCatalogo,
                PrecioBase = model.PrecioBase,
                AplicaIva = model.AplicaIva,
                Disponible24h = model.Disponible24h,
                HoraInicio = model.HoraInicio,
                HoraFin = model.HoraFin,
                IconoUrl = model.IconoUrl,
                EstadoCatalogo = model.EstadoCatalogo,
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

        public static CatalogoServicioDataModel? ToDataModel(this CatalogoServicioDTO? dto)
        {
            if (dto == null) return null;

            return new CatalogoServicioDataModel
            {
                IdCatalogo = dto.IdCatalogo,
                CatalogoGuid = dto.CatalogoGuid,
                IdSucursal = dto.IdSucursal,
                CodigoCatalogo = dto.CodigoCatalogo,
                NombreCatalogo = dto.NombreCatalogo,
                TipoCatalogo = dto.TipoCatalogo,
                CategoriaCatalogo = dto.CategoriaCatalogo,
                DescripcionCatalogo = dto.DescripcionCatalogo,
                PrecioBase = dto.PrecioBase,
                AplicaIva = dto.AplicaIva,
                Disponible24h = dto.Disponible24h,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                IconoUrl = dto.IconoUrl,
                EstadoCatalogo = dto.EstadoCatalogo,
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

        public static List<CatalogoServicioDTO> ToDtoList(this IEnumerable<CatalogoServicioDataModel> models)
            => models?.Select(x => x.ToDto()!).ToList() ?? new();

        public static CatalogoServicioDataModel ToDataModel(this CatalogoServicioCreateDTO dto)
        {
            if (dto == null) return null;

            return new CatalogoServicioDataModel
            {
                IdSucursal = dto.IdSucursal,
                CodigoCatalogo = dto.CodigoCatalogo,
                NombreCatalogo = dto.NombreCatalogo,
                TipoCatalogo = dto.TipoCatalogo,
                CategoriaCatalogo = dto.CategoriaCatalogo ?? string.Empty,
                DescripcionCatalogo = dto.DescripcionCatalogo ?? string.Empty,
                PrecioBase = dto.PrecioBase,
                AplicaIva = dto.AplicaIva,
                Disponible24h = dto.Disponible24h,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                IconoUrl = dto.IconoUrl ?? string.Empty,
                EstadoCatalogo = dto.EstadoCatalogo
            };
        }

        public static CatalogoServicioDataModel ToDataModel(this CatalogoServicioUpdateDTO dto)
        {
            if (dto == null) return null;

            return new CatalogoServicioDataModel
            {
                IdCatalogo = dto.IdCatalogo,
                IdSucursal = dto.IdSucursal,
                CodigoCatalogo = dto.CodigoCatalogo,
                NombreCatalogo = dto.NombreCatalogo,
                TipoCatalogo = dto.TipoCatalogo,
                CategoriaCatalogo = dto.CategoriaCatalogo ?? string.Empty,
                DescripcionCatalogo = dto.DescripcionCatalogo ?? string.Empty,
                PrecioBase = dto.PrecioBase,
                AplicaIva = dto.AplicaIva,
                Disponible24h = dto.Disponible24h,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin,
                IconoUrl = dto.IconoUrl ?? string.Empty,
                EstadoCatalogo = dto.EstadoCatalogo
            };
        }
    }
}
