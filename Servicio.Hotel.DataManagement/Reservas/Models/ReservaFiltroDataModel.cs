using System;

namespace Servicio.Hotel.DataManagement.Reservas.Models
{
    public class ReservaFiltroDataModel
    {
        public int? IdCliente { get; set; }
        public int? IdSucursal { get; set; }
        public string EstadoReserva { get; set; }
        public DateTime? FechaInicioDesde { get; set; }
        public DateTime? FechaInicioHasta { get; set; }
        public string CodigoReserva { get; set; }
        public bool? EsWalkin { get; set; }
        public bool? EsEliminado { get; set; }
    }
}