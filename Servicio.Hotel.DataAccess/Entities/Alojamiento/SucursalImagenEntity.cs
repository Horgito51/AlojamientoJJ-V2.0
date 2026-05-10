using System;

namespace Servicio.Hotel.DataAccess.Entities.Alojamiento
{
    public class SucursalImagenEntity
    {
        public int IdSucursalImagen { get; set; }
        public Guid GuidSucursalImagen { get; set; }
        public int IdSucursal { get; set; }
        public string UrlImagen { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int Orden { get; set; }
        public string Estado { get; set; } = "ACT";
        public DateTime FechaCreacionUtc { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }

        public SucursalEntity Sucursal { get; set; }
    }
}
