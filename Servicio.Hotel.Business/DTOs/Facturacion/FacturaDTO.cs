using System;
using System.Collections.Generic;

namespace Servicio.Hotel.Business.DTOs.Facturacion
{
    public class FacturaDTO
    {
        public int IdFactura { get; set; }
        public Guid GuidFactura { get; set; }
        public int IdCliente { get; set; }
        public int IdReserva { get; set; }
        public int IdSucursal { get; set; }
        public string NumeroFactura { get; set; }
        public string TipoFactura { get; set; } = string.Empty;  // RESERVA, FINAL, AJUSTE
        public DateTime FechaEmision { get; set; }
        public decimal Subtotal { get; set; }
        public decimal ValorIva { get; set; }
        public decimal DescuentoTotal { get; set; }
        public decimal Total { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string Moneda { get; set; }
        public string ObservacionesFactura { get; set; }
        public string OrigenCanalFactura { get; set; }
        public string Estado { get; set; }  // EMI, PAG, ANU
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public bool EsEliminado { get; set; }
        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public string MotivoInhabilitacion { get; set; }
        public byte[] RowVersion { get; set; }

        public List<FacturaDetalleDTO> Detalles { get; set; }
    }
}