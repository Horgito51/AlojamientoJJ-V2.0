using System;

namespace Servicio.Hotel.DataManagement.Alojamiento.Models
{
    public class ImagenDataModel
    {
        public Guid ImagenGuid { get; set; }
        public int? IdImagen { get; set; }
        public string UrlImagen { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int Orden { get; set; }
        public bool EsPrincipal { get; set; }
    }
}
