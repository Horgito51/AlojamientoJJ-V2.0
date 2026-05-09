using System;

namespace Servicio.Hotel.Business.DTOs.Facturacion
{
    public class PagoDTO
    {
        public int IdPago { get; set; }
        public Guid PagoGuid { get; set; }
        public int IdFactura { get; set; }
        public int IdReserva { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
        public bool EsPagoElectronico { get; set; }
        public string ProveedorPasarela { get; set; }
        public string TransaccionExterna { get; set; }
        public string CodigoAutorizacion { get; set; }
        public string Referencia { get; set; }
        public string EstadoPago { get; set; }  // PEN, PRO, APR, REC, CAN
        public DateTime FechaPagoUtc { get; set; }
        public string Moneda { get; set; }
        public decimal TipoCambio { get; set; }
        public string RespuestaPasarela { get; set; }
        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class PagoSimuladoDTO
    {
        public int IdReserva { get; set; }
        public string CodigoReserva { get; set; }
        public decimal Monto { get; set; }
        public string EstadoPago { get; set; }
        public string EstadoReserva { get; set; }
        public string TransaccionExterna { get; set; }
        public string CodigoAutorizacion { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaPagoUtc { get; set; }
    }
}
