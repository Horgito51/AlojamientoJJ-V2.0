using System;

namespace Servicio.Hotel.DataManagement.Facturacion.Models
{
    public class FacturaDetalleDataModel
    {
        public int IdFacturaDetalle { get; set; }
        public Guid FacturaDetalleGuid { get; set; }
        public int IdFactura { get; set; }
        public string TipoItem { get; set; }
        public string ReferenciaTipo { get; set; }
        public int? ReferenciaId { get; set; }
        public string DescripcionItem { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubtotalLinea { get; set; }
        public decimal ValorIvaLinea { get; set; }
        public decimal DescuentoLinea { get; set; }
        public decimal TotalLinea { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }
        public byte[] RowVersion { get; set; }
    }
}