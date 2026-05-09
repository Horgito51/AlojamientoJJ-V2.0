using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Facturacion;
using Servicio.Hotel.DataManagement.Facturacion.Models;

namespace Servicio.Hotel.DataManagement.Facturacion.Mappers
{
    public static class FacturaDataMapper
    {
        public static FacturaDataModel ToModel(this FacturaEntity entity)
        {
            if (entity == null) return null;

            return new FacturaDataModel
            {
                IdFactura = entity.IdFactura,
                GuidFactura = entity.GuidFactura,
                IdCliente = entity.IdCliente,
                IdReserva = entity.IdReserva,
                IdSucursal = entity.IdSucursal,
                NumeroFactura = entity.NumeroFactura,
                TipoFactura = entity.TipoFactura,
                FechaEmision = entity.FechaEmision,
                Subtotal = entity.Subtotal,
                ValorIva = entity.ValorIva,
                DescuentoTotal = entity.DescuentoTotal,
                Total = entity.Total,
                SaldoPendiente = entity.SaldoPendiente,
                Moneda = entity.Moneda,
                ObservacionesFactura = entity.ObservacionesFactura,
                OrigenCanalFactura = entity.OrigenCanalFactura,
                Estado = entity.Estado,
                FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
                EsEliminado = entity.EsEliminado,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                ServicioOrigen = entity.ServicioOrigen,
                MotivoInhabilitacion = entity.MotivoInhabilitacion,
                RowVersion = entity.RowVersion,
                Detalles = entity.FacturaDetalles?.Select(d => d.ToModel()).ToList()
            };
        }

        public static FacturaEntity ToEntity(this FacturaDataModel model)
        {
            if (model == null) return null;

            return new FacturaEntity
            {
                IdFactura = model.IdFactura,
                GuidFactura = model.GuidFactura,
                IdCliente = model.IdCliente,
                IdReserva = model.IdReserva,
                IdSucursal = model.IdSucursal,
                NumeroFactura = model.NumeroFactura,
                TipoFactura = model.TipoFactura,
                FechaEmision = model.FechaEmision,
                Subtotal = model.Subtotal,
                ValorIva = model.ValorIva,
                DescuentoTotal = model.DescuentoTotal,
                Total = model.Total,
                SaldoPendiente = model.SaldoPendiente,
                Moneda = model.Moneda,
                ObservacionesFactura = model.ObservacionesFactura,
                OrigenCanalFactura = model.OrigenCanalFactura,
                Estado = model.Estado,
                FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
                EsEliminado = model.EsEliminado,
                CreadoPorUsuario = model.CreadoPorUsuario,
                FechaRegistroUtc = model.FechaRegistroUtc,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                ServicioOrigen = model.ServicioOrigen,
                MotivoInhabilitacion = model.MotivoInhabilitacion,
                RowVersion = model.RowVersion,
                FacturaDetalles = model.Detalles?.Select(d => d.ToEntity()).ToList()
            };
        }

        public static FacturaDetalleDataModel ToModel(this FacturaDetalleEntity entity)
        {
            if (entity == null) return null;

            return new FacturaDetalleDataModel
            {
                IdFacturaDetalle = entity.IdFacturaDetalle,
                FacturaDetalleGuid = entity.FacturaDetalleGuid,
                IdFactura = entity.IdFactura,
                TipoItem = entity.TipoItem,
                ReferenciaTipo = entity.ReferenciaTipo,
                ReferenciaId = entity.ReferenciaId,
                DescripcionItem = entity.DescripcionItem,
                Cantidad = entity.Cantidad,
                PrecioUnitario = entity.PrecioUnitario,
                SubtotalLinea = entity.SubtotalLinea,
                ValorIvaLinea = entity.ValorIvaLinea,
                DescuentoLinea = entity.DescuentoLinea,
                TotalLinea = entity.TotalLinea,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                RowVersion = entity.RowVersion
            };
        }

        public static FacturaDetalleEntity ToEntity(this FacturaDetalleDataModel model)
        {
            if (model == null) return null;

            return new FacturaDetalleEntity
            {
                IdFacturaDetalle = model.IdFacturaDetalle,
                FacturaDetalleGuid = model.FacturaDetalleGuid,
                IdFactura = model.IdFactura,
                TipoItem = model.TipoItem,
                ReferenciaTipo = model.ReferenciaTipo,
                ReferenciaId = model.ReferenciaId,
                DescripcionItem = model.DescripcionItem,
                Cantidad = model.Cantidad,
                PrecioUnitario = model.PrecioUnitario,
                SubtotalLinea = model.SubtotalLinea,
                ValorIvaLinea = model.ValorIvaLinea,
                DescuentoLinea = model.DescuentoLinea,
                TotalLinea = model.TotalLinea,
                FechaRegistroUtc = model.FechaRegistroUtc,
                CreadoPorUsuario = model.CreadoPorUsuario,
                RowVersion = model.RowVersion
            };
        }

        public static List<FacturaDataModel> ToModelList(this IEnumerable<FacturaEntity> entities)
            => entities?.Select(e => e.ToModel()).ToList() ?? new();

        public static List<FacturaEntity> ToEntityList(this IEnumerable<FacturaDataModel> models)
            => models?.Select(m => m.ToEntity()).ToList() ?? new();
    }
}