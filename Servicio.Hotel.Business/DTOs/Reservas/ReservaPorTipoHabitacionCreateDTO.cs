using System;
using System.Collections.Generic;

namespace Servicio.Hotel.Business.DTOs.Reservas
{
    public sealed class ReservaPorTipoHabitacionCreateDTO
    {
        public int IdCliente { get; set; }
        public int IdSucursal { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string OrigenCanalReserva { get; set; } = "MARKETPLACE";
        public string? Observaciones { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public bool EsWalkin { get; set; }
        public bool ExigirPermiteReservaPublica { get; set; } = true;
        public List<ReservaTipoHabitacionCreateDTO> Habitaciones { get; set; } = new();
    }

    public sealed class ReservaTipoHabitacionCreateDTO
    {
        public Guid TipoHabitacionGuid { get; set; }
        public int NumHabitaciones { get; set; }
        public int NumAdultos { get; set; }
        public int NumNinos { get; set; }
    }
}
