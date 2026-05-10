using System;
using System.Collections.Generic;

namespace Servicio.Hotel.Business.DTOs.Alojamiento
{
    public class SucursalDTO
    {
        public int IdSucursal { get; set; }
        public Guid SucursalGuid { get; set; }
        public string CodigoSucursal { get; set; } = string.Empty;
        public string NombreSucursal { get; set; } = string.Empty;
        public string DescripcionSucursal { get; set; } = string.Empty;
        public string DescripcionCorta { get; set; } = string.Empty;
        public string TipoAlojamiento { get; set; } = string.Empty;
        public int? Estrellas { get; set; }
        public string CategoriaViaje { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public string HoraCheckin { get; set; } = string.Empty;
        public string HoraCheckout { get; set; } = string.Empty;
        public bool CheckinAnticipado { get; set; }
        public bool CheckoutTardio { get; set; }
        public bool AceptaNinos { get; set; }
        public int? EdadMinimaHuesped { get; set; }
        public bool PermiteMascotas { get; set; }
        public bool SePermiteFumar { get; set; }
        public string EstadoSucursal { get; set; } = string.Empty;
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
        public List<ImagenDTO> Imagenes { get; set; } = new();
    }
}
