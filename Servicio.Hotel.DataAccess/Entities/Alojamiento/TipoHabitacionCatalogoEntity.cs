using System;

namespace Servicio.Hotel.DataAccess.Entities.Alojamiento
{
    public class TipoHabitacionCatalogoEntity
    {
        public int IdTipoHabCatalogo { get; set; }
        public int IdTipoHabitacion { get; set; }
        public int IdCatalogo { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }
        public byte[] RowVersion { get; set; }

        // Navigation properties
        public TipoHabitacionEntity TipoHabitacion { get; set; }
        public CatalogoServicioEntity CatalogoServicio { get; set; }
    }
}