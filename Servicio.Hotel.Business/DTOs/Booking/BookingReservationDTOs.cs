using System;
using System.Collections.Generic;

namespace Servicio.Hotel.Business.DTOs.Booking
{
    public sealed class CrearReservaPublicRequestDTO
    {
        public Guid? ClienteGuid { get; set; }
        public Guid SucursalGuid { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string OrigenCanalReserva { get; set; } = "MARKETPLACE";
        public string? Observaciones { get; set; }
        public ClientePublicRequestDTO Cliente { get; set; } = new();
        public List<ReservaHabitacionPublicRequestDTO> Habitaciones { get; set; } = new();
    }

    public sealed class ClientePublicRequestDTO
    {
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string NumeroIdentificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string? Apellidos { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
    }

    public sealed class ReservaHabitacionPublicRequestDTO
    {
        public Guid TipoHabitacionGuid { get; set; }
        public int NumHabitaciones { get; set; } = 1;
        public int NumAdultos { get; set; } = 1;
        public int NumNinos { get; set; }
    }

    public sealed class ReservaPublicContractDTO
    {
        public Guid ReservaGuid { get; set; }
        public string CodigoReserva { get; set; } = string.Empty;
        public Guid ClienteGuid { get; set; }
        public Guid SucursalGuid { get; set; }
        public DateTime FechaReservaUtc { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal SubtotalReserva { get; set; }
        public decimal ValorIva { get; set; }
        public decimal TotalReserva { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string OrigenCanalReserva { get; set; } = string.Empty;
        public string EstadoReserva { get; set; } = string.Empty;
        public DateTime? FechaConfirmacionUtc { get; set; }
        public string? Observaciones { get; set; }
        public bool EsWalkin { get; set; }
        public List<ReservaHabitacionPublicContractDTO> Habitaciones { get; set; } = new();
    }

    public sealed class ReservaHabitacionPublicContractDTO
    {
        public Guid ReservaHabitacionGuid { get; set; }
        public Guid HabitacionGuid { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int NumAdultos { get; set; }
        public int NumNinos { get; set; }
        public decimal PrecioNocheAplicado { get; set; }
        public decimal SubtotalLinea { get; set; }
        public decimal ValorIvaLinea { get; set; }
        public decimal DescuentoLinea { get; set; }
        public decimal TotalLinea { get; set; }
        public string EstadoDetalle { get; set; } = string.Empty;
    }
}
