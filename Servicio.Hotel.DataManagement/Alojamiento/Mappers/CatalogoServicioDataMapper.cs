using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.DataManagement.Alojamiento.Mappers
{
    public static class CatalogoServicioDataMapper
    {
        public static CatalogoServicioDataModel? ToModel(this CatalogoServicioEntity? entity)
        {
            if (entity == null) return null;

            return new CatalogoServicioDataModel
            {
                IdCatalogo = entity.IdCatalogo,
                CatalogoGuid = entity.CatalogoGuid,
                IdSucursal = entity.IdSucursal,
                CodigoCatalogo = entity.CodigoCatalogo,
                NombreCatalogo = entity.NombreCatalogo,
                TipoCatalogo = entity.TipoCatalogo,
                CategoriaCatalogo = entity.CategoriaCatalogo,
                DescripcionCatalogo = entity.DescripcionCatalogo,
                PrecioBase = entity.PrecioBase,
                AplicaIva = entity.AplicaIva,
                Disponible24h = entity.Disponible24h,
                HoraInicio = entity.HoraInicio,
                HoraFin = entity.HoraFin,
                IconoUrl = entity.IconoUrl,
                EstadoCatalogo = entity.EstadoCatalogo,
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

        public static CatalogoServicioEntity? ToEntity(this CatalogoServicioDataModel? model)
        {
            if (model == null) return null;

            return new CatalogoServicioEntity
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

        public static List<CatalogoServicioDataModel> ToModelList(this IEnumerable<CatalogoServicioEntity> entities)
            => entities?.Select(x => x.ToModel()!).ToList() ?? new();
    }
}
