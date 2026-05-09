using System;

namespace Servicio.Hotel.Business.DTOs.Reservas
{
    public class ReservaPrecioDTO
    {
        public int IdHabitacion { get; set; }
        public Guid HabitacionGuid { get; set; }
        public int IdSucursal { get; set; }
        public int? IdTarifa { get; set; }
        public decimal PrecioNocheAplicado { get; set; }
        public decimal SubtotalLinea { get; set; }
        public decimal ValorIvaLinea { get; set; }
        public decimal TotalLinea { get; set; }
        public string OrigenPrecio { get; set; } = "PRECIO_BASE";
    }
}
