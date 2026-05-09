using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.Business.DTOs.Facturacion;
using Servicio.Hotel.DataManagement.Facturacion.Models;

namespace Servicio.Hotel.Business.Mappers.Facturacion
{
    public static class FacturaBusinessMapper
    {
        public static FacturaDTO ToDto(this FacturaDataModel model)
        {
            if (model == null) return null;

            return new FacturaDTO
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
                Detalles = model.Detalles?.Select(d => d.ToDto()).ToList()
            };
        }

        public static FacturaDataModel ToDataModel(this FacturaDTO dto)
        {
            if (dto == null) return null;

            return new FacturaDataModel
            {
                IdFactura = dto.IdFactura,
                GuidFactura = dto.GuidFactura,
                IdCliente = dto.IdCliente,
                IdReserva = dto.IdReserva,
                IdSucursal = dto.IdSucursal,
                NumeroFactura = dto.NumeroFactura,
                TipoFactura = dto.TipoFactura,
                FechaEmision = dto.FechaEmision,
                Subtotal = dto.Subtotal,
                ValorIva = dto.ValorIva,
                DescuentoTotal = dto.DescuentoTotal,
                Total = dto.Total,
                SaldoPendiente = dto.SaldoPendiente,
                Moneda = dto.Moneda,
                ObservacionesFactura = dto.ObservacionesFactura,
                OrigenCanalFactura = dto.OrigenCanalFactura,
                Estado = dto.Estado,
                FechaInhabilitacionUtc = dto.FechaInhabilitacionUtc,
                EsEliminado = dto.EsEliminado,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                ModificadoPorUsuario = dto.ModificadoPorUsuario,
                FechaModificacionUtc = dto.FechaModificacionUtc,
                ModificacionIp = dto.ModificacionIp,
                ServicioOrigen = dto.ServicioOrigen,
                MotivoInhabilitacion = dto.MotivoInhabilitacion,
                RowVersion = dto.RowVersion,
                Detalles = dto.Detalles?.Select(d => d.ToDataModel()).ToList()
            };
        }

        public static List<FacturaDTO> ToDtoList(this IEnumerable<FacturaDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<FacturaDataModel> ToDataModelList(this IEnumerable<FacturaDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();

        // Mapeo para FacturaDetalle
        public static FacturaDetalleDTO ToDto(this FacturaDetalleDataModel model)
        {
            if (model == null) return null;

            return new FacturaDetalleDTO
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

        public static FacturaDetalleDataModel ToDataModel(this FacturaDetalleDTO dto)
        {
            if (dto == null) return null;

            return new FacturaDetalleDataModel
            {
                IdFacturaDetalle = dto.IdFacturaDetalle,
                FacturaDetalleGuid = dto.FacturaDetalleGuid,
                IdFactura = dto.IdFactura,
                TipoItem = dto.TipoItem,
                ReferenciaTipo = dto.ReferenciaTipo,
                ReferenciaId = dto.ReferenciaId,
                DescripcionItem = dto.DescripcionItem,
                Cantidad = dto.Cantidad,
                PrecioUnitario = dto.PrecioUnitario,
                SubtotalLinea = dto.SubtotalLinea,
                ValorIvaLinea = dto.ValorIvaLinea,
                DescuentoLinea = dto.DescuentoLinea,
                TotalLinea = dto.TotalLinea,
                FechaRegistroUtc = dto.FechaRegistroUtc,
                CreadoPorUsuario = dto.CreadoPorUsuario,
                RowVersion = dto.RowVersion
            };
        }

        public static List<FacturaDetalleDTO> ToDtoList(this IEnumerable<FacturaDetalleDataModel> models)
            => models?.Select(m => m.ToDto()).ToList() ?? new();

        public static List<FacturaDetalleDataModel> ToDataModelList(this IEnumerable<FacturaDetalleDTO> dtos)
            => dtos?.Select(d => d.ToDataModel()).ToList() ?? new();

        public static FacturaFiltroDataModel ToDataModel(this FacturaFiltroDTO dto)
        {
            if (dto == null) return null;

            return new FacturaFiltroDataModel
            {
                IdCliente = dto.IdCliente,
                IdReserva = dto.IdReserva,
                IdSucursal = dto.IdSucursal,
                TipoFactura = dto.TipoFactura,
                Estado = dto.Estado,
                FechaEmisionDesde = dto.FechaEmisionDesde,
                FechaEmisionHasta = dto.FechaEmisionHasta,
                NumeroFactura = dto.NumeroFactura,
                EsEliminado = dto.EsEliminado
            };
        }
    }
}