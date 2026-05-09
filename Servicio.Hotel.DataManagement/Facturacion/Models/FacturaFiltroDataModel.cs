using System;

namespace Servicio.Hotel.DataManagement.Facturacion.Models
{
    public class FacturaFiltroDataModel
    {
        public int? IdCliente { get; set; }
        public int? IdReserva { get; set; }
        public int? IdSucursal { get; set; }
        public string TipoFactura { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaEmisionDesde { get; set; }
        public DateTime? FechaEmisionHasta { get; set; }
        public string NumeroFactura { get; set; }
        public bool? EsEliminado { get; set; }
    }
}