namespace Servicio.Hotel.DataManagement.Alojamiento.Models
{
    public class HabitacionFiltroDataModel
    {
        public int? IdSucursal { get; set; }
        public int? IdTipoHabitacion { get; set; }
        public string EstadoHabitacion { get; set; }
        public string NumeroHabitacion { get; set; }
        public bool? EsEliminado { get; set; }
        public bool? IncluirInhabilitados { get; set; } // para ignorar filtro de eliminación
    }
}