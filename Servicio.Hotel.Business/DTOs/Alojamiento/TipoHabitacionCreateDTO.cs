using System;

namespace Servicio.Hotel.Business.DTOs.Alojamiento
{
    public class TipoHabitacionCreateDTO
    {
        public string CodigoTipoHabitacion { get; set; } = string.Empty;
        public string NombreTipoHabitacion { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int CapacidadAdultos { get; set; }
        public int CapacidadNinos { get; set; }
        public int CapacidadTotal { get; set; }
        public string TipoCama { get; set; } = string.Empty;
        public decimal? AreaM2 { get; set; }
        public bool PermiteEventos { get; set; }
        public bool PermiteReservaPublica { get; set; }
        public string EstadoTipoHabitacion { get; set; } = "ACT";
    }
}
