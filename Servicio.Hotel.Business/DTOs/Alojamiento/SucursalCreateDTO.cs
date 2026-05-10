using System;
using System.Collections.Generic;

namespace Servicio.Hotel.Business.DTOs.Alojamiento
{
    public class SucursalCreateDTO
    {
        public string CodigoSucursal { get; set; } = string.Empty;
        public string NombreSucursal { get; set; } = string.Empty;
        public string? DescripcionSucursal { get; set; }
        public string? DescripcionCorta { get; set; }
        public string? TipoAlojamiento { get; set; }
        public int? Estrellas { get; set; }
        public string? CategoriaViaje { get; set; }
        public string? Pais { get; set; }
        public string? Provincia { get; set; }
        public string? Ciudad { get; set; }
        public string? Ubicacion { get; set; }
        public string? Direccion { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
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
        public string EstadoSucursal { get; set; } = "ACT";
        public List<ImagenDTO>? Imagenes { get; set; }
    }
}
