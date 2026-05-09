using System;

namespace Servicio.Hotel.Business.DTOs.Valoraciones
{
    public class ValoracionFiltroDTO
    {
        public int? IdSucursal { get; set; }
        public int? IdHabitacion { get; set; }
        public int? IdCliente { get; set; }
        public string EstadoValoracion { get; set; }
        public string TipoViaje { get; set; }
        public bool? PublicadaEnPortal { get; set; }
        public decimal? PuntuacionMin { get; set; }
        public decimal? PuntuacionMax { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}