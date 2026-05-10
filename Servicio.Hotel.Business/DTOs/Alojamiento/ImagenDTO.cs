using System;

namespace Servicio.Hotel.Business.DTOs.Alojamiento
{
    public class ImagenDTO
    {
        public Guid ImagenGuid { get; set; }
        public int? IdImagen { get; set; }
        public string UrlImagen { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int Orden { get; set; }
        public bool EsPrincipal { get; set; }
    }
}
