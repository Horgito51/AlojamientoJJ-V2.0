using System;
using System.Collections.Generic;

namespace Servicio.Hotel.Business.DTOs.Reservas
{
    public class ReservaDTO
    {
        public int IdReserva { get; set; }
        public Guid GuidReserva { get; set; }
        public string CodigoReserva { get; set; }
        public int IdCliente { get; set; }
        public int IdSucursal { get; set; }
        public DateTime FechaReservaUtc { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal SubtotalReserva { get; set; }
        public decimal ValorIva { get; set; }
        public decimal TotalReserva { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string OrigenCanalReserva { get; set; }
        public string EstadoReserva { get; set; }
        public DateTime? FechaConfirmacionUtc { get; set; }
        public DateTime? FechaCancelacionUtc { get; set; }
        public string MotivoCancelacion { get; set; }
        public string Observaciones { get; set; }
        public bool EsWalkin { get; set; }
        public bool EsEliminado { get; set; }
        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string MotivoInhabilitacion { get; set; }
        public byte[] RowVersion { get; set; }

        public List<ReservaHabitacionDTO> Habitaciones { get; set; }
    }
}