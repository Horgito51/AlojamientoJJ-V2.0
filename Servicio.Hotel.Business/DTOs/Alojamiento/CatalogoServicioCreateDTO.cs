using System;

namespace Servicio.Hotel.Business.DTOs.Alojamiento
{
    public class CatalogoServicioCreateDTO
    {
        public int? IdSucursal { get; set; }
        public string CodigoCatalogo { get; set; } = string.Empty;
        public string NombreCatalogo { get; set; } = string.Empty;
        public string TipoCatalogo { get; set; } = string.Empty;
        public string? CategoriaCatalogo { get; set; }
        public string? DescripcionCatalogo { get; set; }
        public decimal PrecioBase { get; set; }
        public bool AplicaIva { get; set; }
        public bool Disponible24h { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }
        public string? IconoUrl { get; set; }
        public string EstadoCatalogo { get; set; } = "ACT";
    }
}
