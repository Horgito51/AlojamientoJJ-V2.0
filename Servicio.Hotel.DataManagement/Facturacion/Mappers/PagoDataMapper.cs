using System.Collections.Generic;
using System.Linq;
using Servicio.Hotel.DataAccess.Entities.Facturacion;
using Servicio.Hotel.DataManagement.Facturacion.Models;

namespace Servicio.Hotel.DataManagement.Facturacion.Mappers
{
    public static class PagoDataMapper
    {
        public static PagoDataModel ToModel(this PagoEntity entity)
        {
            if (entity == null) return null;

            return new PagoDataModel
            {
                IdPago = entity.IdPago,
                PagoGuid = entity.PagoGuid,
                IdFactura = entity.IdFactura,
                IdReserva = entity.IdReserva,
                Monto = entity.Monto,
                MetodoPago = entity.MetodoPago,
                EsPagoElectronico = entity.EsPagoElectronico,
                ProveedorPasarela = entity.ProveedorPasarela,
                TransaccionExterna = entity.TransaccionExterna,
                CodigoAutorizacion = entity.CodigoAutorizacion,
                Referencia = entity.Referencia,
                EstadoPago = entity.EstadoPago,
                FechaPagoUtc = entity.FechaPagoUtc,
                Moneda = entity.Moneda,
                TipoCambio = entity.TipoCambio,
                RespuestaPasarela = entity.RespuestaPasarela,
                CreadoPorUsuario = entity.CreadoPorUsuario,
                FechaRegistroUtc = entity.FechaRegistroUtc,
                ModificadoPorUsuario = entity.ModificadoPorUsuario,
                FechaModificacionUtc = entity.FechaModificacionUtc,
                ModificacionIp = entity.ModificacionIp,
                ServicioOrigen = entity.ServicioOrigen,
                RowVersion = entity.RowVersion
            };
        }

        public static PagoEntity ToEntity(this PagoDataModel model)
        {
            if (model == null) return null;

            return new PagoEntity
            {
                IdPago = model.IdPago,
                PagoGuid = model.PagoGuid,
                IdFactura = model.IdFactura,
                IdReserva = model.IdReserva,
                Monto = model.Monto,
                MetodoPago = model.MetodoPago,
                EsPagoElectronico = model.EsPagoElectronico,
                ProveedorPasarela = model.ProveedorPasarela,
                TransaccionExterna = model.TransaccionExterna,
                CodigoAutorizacion = model.CodigoAutorizacion,
                Referencia = model.Referencia,
                EstadoPago = model.EstadoPago,
                FechaPagoUtc = model.FechaPagoUtc,
                Moneda = model.Moneda,
                TipoCambio = model.TipoCambio,
                RespuestaPasarela = model.RespuestaPasarela,
                CreadoPorUsuario = model.CreadoPorUsuario,
                FechaRegistroUtc = model.FechaRegistroUtc,
                ModificadoPorUsuario = model.ModificadoPorUsuario,
                FechaModificacionUtc = model.FechaModificacionUtc,
                ModificacionIp = model.ModificacionIp,
                ServicioOrigen = model.ServicioOrigen,
                RowVersion = model.RowVersion
            };
        }

        public static List<PagoDataModel> ToModelList(this IEnumerable<PagoEntity> entities)
            => entities?.Select(e => e.ToModel()).ToList() ?? new();

        public static List<PagoEntity> ToEntityList(this IEnumerable<PagoDataModel> models)
            => models?.Select(m => m.ToEntity()).ToList() ?? new();
    }
}