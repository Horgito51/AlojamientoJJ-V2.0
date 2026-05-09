using System;
using System.Collections.Generic;

namespace Servicio.Hotel.DataAccess.Entities.Alojamiento
{
    public class CatalogoServicioEntity
    {
        public int IdCatalogo { get; set; }
        public Guid CatalogoGuid { get; set; }
        public int? IdSucursal { get; set; }
        public string CodigoCatalogo { get; set; }
        public string NombreCatalogo { get; set; }
        public string TipoCatalogo { get; set; }
        public string CategoriaCatalogo { get; set; }
        public string? DescripcionCatalogo { get; set; }
        public decimal PrecioBase { get; set; }
        public bool AplicaIva { get; set; }
        public bool Disponible24h { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }
        public string? IconoUrl { get; set; }
        public string EstadoCatalogo { get; set; }
        public bool EsEliminado { get; set; }
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string? MotivoInhabilitacion { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }
        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }
        public string ServicioOrigen { get; set; }
        public byte[] RowVersion { get; set; }

        public SucursalEntity Sucursal { get; set; }
        public ICollection<TipoHabitacionCatalogoEntity> TipoHabitacionCatalogos { get; set; }
    }
}
