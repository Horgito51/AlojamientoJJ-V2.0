using System;

namespace Servicio.Hotel.DataAccess.Entities.Facturacion
{
    public class PagoEntity
    {
        public int IdPago { get; set; }
        public Guid PagoGuid { get; set; }
        public int IdFactura { get; set; }
        public int IdReserva { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
        public bool EsPagoElectronico { get; set; }
        public string? ProveedorPasarela { get; set; }
        public string? TransaccionExterna { get; set; }
        public string? CodigoAutorizacion { get; set; }
        public string? Referencia { get; set; }
        public string EstadoPago { get; set; }
        public DateTime FechaPagoUtc { get; set; }
        public string Moneda { get; set; }
        public decimal TipoCambio { get; set; }
        public string? RespuestaPasarela { get; set; }
        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public byte[] RowVersion { get; set; }

        public FacturaEntity Factura { get; set; }
    }
}
