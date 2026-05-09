using System;

namespace Servicio.Hotel.DataManagement.Valoraciones.Models
{
    public class ValoracionFiltroDataModel
    {
        public int? IdSucursal { get; set; }
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