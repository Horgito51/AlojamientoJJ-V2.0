using System;

namespace Servicio.Hotel.Business.DTOs.Hospedaje
{
    public class CargoEstadiaDTO
    {
        public int IdCargoEstadia { get; set; }
        public Guid CargoGuid { get; set; }
        public int IdEstadia { get; set; }
        public int? IdCatalogo { get; set; }
        public string DescripcionCargo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal ValorIva { get; set; }
        public decimal TotalCargo { get; set; }
        public DateTime FechaConsumoUtc { get; set; }
        public string EstadoCargo { get; set; }  // PEN, FAC, ANU
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }
        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public byte[] RowVersion { get; set; }
    }
}