using System;

namespace Servicio.Hotel.Business.DTOs.Alojamiento
{
    public class TarifaCreateDTO
    {
        public string CodigoTarifa { get; set; } = string.Empty;
        public int IdSucursal { get; set; }
        public int IdTipoHabitacion { get; set; }
        public string NombreTarifa { get; set; } = string.Empty;
        public string CanalTarifa { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal PrecioPorNoche { get; set; }
        public decimal PorcentajeIva { get; set; }
        public int MinNoches { get; set; }
        public int? MaxNoches { get; set; }
        public bool PermitePortalPublico { get; set; }
        public int Prioridad { get; set; }
        public string EstadoTarifa { get; set; } = "ACT";
    }
}
