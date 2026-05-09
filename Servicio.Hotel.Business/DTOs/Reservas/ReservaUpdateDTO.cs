using System;
using System.Collections.Generic;

namespace Servicio.Hotel.Business.DTOs.Reservas
{
    public class ReservaUpdateDTO
    {
        public int IdReserva { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal SubtotalReserva { get; set; }
        public decimal ValorIva { get; set; }
        public decimal TotalReserva { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string EstadoReserva { get; set; } = "PEN";
        public string? Observaciones { get; set; }
    }
}
