using System;

namespace Servicio.Hotel.DataManagement.Alojamiento.Models
{
    public class CatalogoServicioDataModel
    {
        public int IdCatalogo { get; set; }
        public Guid CatalogoGuid { get; set; }
        public int? IdSucursal { get; set; }
        public string CodigoCatalogo { get; set; } = string.Empty;
        public string NombreCatalogo { get; set; } = string.Empty;
        public string TipoCatalogo { get; set; } = string.Empty;
        public string CategoriaCatalogo { get; set; } = string.Empty;
        public string DescripcionCatalogo { get; set; } = string.Empty;
        public decimal PrecioBase { get; set; }
        public bool AplicaIva { get; set; }
        public bool Disponible24h { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }
        public string IconoUrl { get; set; } = string.Empty;
        public string EstadoCatalogo { get; set; } = string.Empty;
        public bool EsEliminado { get; set; }
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string MotivoInhabilitacion { get; set; } = string.Empty;
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; } = string.Empty;
        public string ModificadoPorUsuario { get; set; } = string.Empty;
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; } = string.Empty;
        public string ServicioOrigen { get; set; } = string.Empty;
        public byte[]? RowVersion { get; set; }
    }
}
