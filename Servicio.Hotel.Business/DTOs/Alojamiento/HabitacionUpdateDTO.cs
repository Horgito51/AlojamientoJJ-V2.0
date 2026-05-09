using System;

namespace Servicio.Hotel.Business.DTOs.Alojamiento
{
    public class HabitacionUpdateDTO
    {
        public int IdHabitacion { get; set; }
        public int IdSucursal { get; set; }
        public int IdTipoHabitacion { get; set; }
        public string NumeroHabitacion { get; set; } = string.Empty;
        public int? Piso { get; set; }
        public int CapacidadHabitacion { get; set; }
        public decimal PrecioBase { get; set; }
        public string? Url { get; set; }
        public string? DescripcionHabitacion { get; set; }
        public string EstadoHabitacion { get; set; } = "DIS";
    }
}
