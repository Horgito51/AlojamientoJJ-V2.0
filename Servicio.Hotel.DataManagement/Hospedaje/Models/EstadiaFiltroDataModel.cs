using System;

namespace Servicio.Hotel.DataManagement.Hospedaje.Models
{
    public class EstadiaFiltroDataModel
    {
        public int? IdReservaHabitacion { get; set; }
        public int? IdCliente { get; set; }
        public int? IdHabitacion { get; set; }
        public string EstadoEstadia { get; set; }
        public DateTime? CheckinDesde { get; set; }
        public DateTime? CheckinHasta { get; set; }
        public bool? RequiereMantenimiento { get; set; }
    }
}