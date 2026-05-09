using System;

namespace Servicio.Hotel.DataAccess.Entities.Alojamiento
{
    public class TipoHabitacionImagenEntity
    {
        public int IdTipoHabitacionImagen { get; set; }
        public int IdTipoHabitacion { get; set; }
        public string UrlImagen { get; set; }
        public string DescripcionImagen { get; set; }
        public int OrdenVisualizacion { get; set; }
        public bool EsPrincipal { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public TipoHabitacionEntity TipoHabitacion { get; set; }
    }
}