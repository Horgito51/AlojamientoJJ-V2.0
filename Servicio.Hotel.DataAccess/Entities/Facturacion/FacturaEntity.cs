using System;
using System.Collections.Generic;

namespace Servicio.Hotel.DataAccess.Entities.Facturacion
{
    public class FacturaEntity
    {
        public int IdFactura { get; set; }
        public Guid GuidFactura { get; set; }
        public int IdCliente { get; set; }
        public int IdReserva { get; set; }
        public int IdSucursal { get; set; }
        public string NumeroFactura { get; set; }
        public string TipoFactura { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Subtotal { get; set; }
        public decimal ValorIva { get; set; }
        public decimal DescuentoTotal { get; set; }
        public decimal Total { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string Moneda { get; set; }
        public string? ObservacionesFactura { get; set; }
        public string? OrigenCanalFactura { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public bool EsEliminado { get; set; }
        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public string? MotivoInhabilitacion { get; set; }
        public byte[] RowVersion { get; set; }

        public ICollection<FacturaDetalleEntity> FacturaDetalles { get; set; }
        public ICollection<PagoEntity> Pagos { get; set; }
    }
}
