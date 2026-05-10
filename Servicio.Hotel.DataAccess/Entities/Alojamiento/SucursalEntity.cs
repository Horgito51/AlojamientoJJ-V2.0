using System;
using System.Collections.Generic;

namespace Servicio.Hotel.DataAccess.Entities.Alojamiento
{
    public class SucursalEntity
    {
        public int IdSucursal { get; set; }
        public Guid SucursalGuid { get; set; }
        public string CodigoSucursal { get; set; }
        public string NombreSucursal { get; set; }
        public string? DescripcionSucursal { get; set; }
        public string? DescripcionCorta { get; set; }
        public string TipoAlojamiento { get; set; }
        public int? Estrellas { get; set; }
        public string? CategoriaViaje { get; set; }
        public string Pais { get; set; }
        public string? Provincia { get; set; }
        public string Ciudad { get; set; }
        public string Ubicacion { get; set; }
        public string Direccion { get; set; }
        public string? CodigoPostal { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public string? HoraCheckin { get; set; }
        public string? HoraCheckout { get; set; }
        public bool CheckinAnticipado { get; set; }
        public bool CheckoutTardio { get; set; }
        public bool AceptaNinos { get; set; }
        public int? EdadMinimaHuesped { get; set; }
        public bool PermiteMascotas { get; set; }
        public bool SePermiteFumar { get; set; }
        public string EstadoSucursal { get; set; }
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

        public ICollection<HabitacionEntity> Habitaciones { get; set; }
        public ICollection<TarifaEntity> Tarifas { get; set; }
        public ICollection<CatalogoServicioEntity> CatalogosServicios { get; set; }
        public ICollection<SucursalImagenEntity> Imagenes { get; set; }
    }
}
