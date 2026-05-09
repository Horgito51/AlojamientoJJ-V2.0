using System;

namespace Servicio.Hotel.DataAccess.Entities.Reservas
{
    public class ReservaHabitacionEntity
    {
        public int IdReservaHabitacion { get; set; }
        public Guid ReservaHabitacionGuid { get; set; }
        public int IdReserva { get; set; }
        public int IdHabitacion { get; set; }
        public int? IdTarifa { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int NumAdultos { get; set; }
        public int NumNinos { get; set; }
        public decimal PrecioNocheAplicado { get; set; }
        public decimal SubtotalLinea { get; set; }
        public decimal ValorIvaLinea { get; set; }
        public decimal DescuentoLinea { get; set; }
        public decimal TotalLinea { get; set; }
        public string EstadoDetalle { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }
        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public byte[] RowVersion { get; set; }

        public ReservaEntity Reserva { get; set; }
    }
}
