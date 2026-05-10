using System;

namespace Servicio.Hotel.API.Models.Responses.Public
{
    public sealed class UsuarioPublicDto
    {
        public Guid UsuarioGuid { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string EstadoUsuario { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public sealed class RolPublicDto
    {
        public Guid RolGuid { get; set; }
        public string NombreRol { get; set; } = string.Empty;
        public string? DescripcionRol { get; set; }
        public string EstadoRol { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public sealed class SucursalPublicDto
    {
        public Guid SucursalGuid { get; set; }
        public string CodigoSucursal { get; set; } = string.Empty;
        public string NombreSucursal { get; set; } = string.Empty;
        public string? DescripcionSucursal { get; set; }
        public string TipoAlojamiento { get; set; } = string.Empty;
        public int? Estrellas { get; set; }
        public string? CategoriaViaje { get; set; }
        public string Pais { get; set; } = string.Empty;
        public string? Provincia { get; set; }
        public string Ciudad { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string? HoraCheckin { get; set; }
        public string? HoraCheckout { get; set; }
        public bool CheckinAnticipado { get; set; }
        public bool CheckoutTardio { get; set; }
        public bool AceptaNinos { get; set; }
        public bool PermiteMascotas { get; set; }
        public bool SePermiteFumar { get; set; }
        public string EstadoSucursal { get; set; } = string.Empty;
        public List<ImagenPublicDto> Imagenes { get; set; } = new();
    }

    public sealed class TipoHabitacionPublicDto
    {
        public Guid TipoHabitacionGuid { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string CodigoTipoHabitacion { get; set; } = string.Empty;
        public string NombreTipoHabitacion { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int CapacidadAdultos { get; set; }
        public int CapacidadNinos { get; set; }
        public int CapacidadTotal { get; set; }
        public string? TipoCama { get; set; }
        public decimal? AreaM2 { get; set; }
        public bool PermiteEventos { get; set; }
        public bool PermiteReservaPublica { get; set; }
        public string EstadoTipoHabitacion { get; set; } = string.Empty;
        public List<ImagenPublicDto> Imagenes { get; set; } = new();
    }

    public sealed class HabitacionPublicDto
    {
        public Guid HabitacionGuid { get; set; }
        public string NumeroHabitacion { get; set; } = string.Empty;
        public int? Piso { get; set; }
        public int CapacidadHabitacion { get; set; }
        public decimal PrecioBase { get; set; }
        public string? DescripcionHabitacion { get; set; }
        public string EstadoHabitacion { get; set; } = string.Empty;
        public Guid SucursalGuid { get; set; }
        public Guid TipoHabitacionGuid { get; set; }
        public string TipoHabitacionSlug { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
        public List<ImagenPublicDto> Imagenes { get; set; } = new();
    }

    public sealed class ImagenPublicDto
    {
        public Guid ImagenGuid { get; set; }
        public string UrlImagen { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int Orden { get; set; }
        public bool EsPrincipal { get; set; }
    }

    public sealed class ClientePublicDto
    {
        public Guid ClienteGuid { get; set; }
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string NumeroIdentificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? RazonSocial { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public sealed class ReservaPublicDto
    {
        public Guid ReservaGuid { get; set; }
        public string CodigoReserva { get; set; } = string.Empty;
        public Guid ClienteGuid { get; set; }
        public Guid SucursalGuid { get; set; }
        public DateTime FechaReservaUtc { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal SubtotalReserva { get; set; }
        public decimal ValorIva { get; set; }
        public decimal TotalReserva { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string OrigenCanalReserva { get; set; } = string.Empty;
        public string EstadoReserva { get; set; } = string.Empty;
        public DateTime? FechaConfirmacionUtc { get; set; }
        public DateTime? FechaCancelacionUtc { get; set; }
        public string? MotivoCancelacion { get; set; }
        public string? Observaciones { get; set; }
        public bool EsWalkin { get; set; }
        public List<ReservaHabitacionPublicDto> Habitaciones { get; set; } = new();
    }

    public sealed class ReservaHabitacionPublicDto
    {
        public Guid ReservaHabitacionGuid { get; set; }
        public Guid HabitacionGuid { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int NumAdultos { get; set; }
        public int NumNinos { get; set; }
        public decimal PrecioNocheAplicado { get; set; }
        public decimal SubtotalLinea { get; set; }
        public decimal ValorIvaLinea { get; set; }
        public decimal DescuentoLinea { get; set; }
        public decimal TotalLinea { get; set; }
        public string EstadoDetalle { get; set; } = string.Empty;
    }

    public sealed class ReservaPrecioPublicDto
    {
        public Guid HabitacionGuid { get; set; }
        public decimal PrecioNocheAplicado { get; set; }
        public decimal SubtotalLinea { get; set; }
        public decimal ValorIvaLinea { get; set; }
        public decimal TotalLinea { get; set; }
        public string OrigenPrecio { get; set; } = "PRECIO_BASE";
    }

    public sealed class PagoSimuladoPublicDto
    {
        public Guid ReservaGuid { get; set; }
        public string CodigoReserva { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string EstadoPago { get; set; } = string.Empty;
        public string EstadoReserva { get; set; } = string.Empty;
        public string TransaccionExterna { get; set; } = string.Empty;
        public string CodigoAutorizacion { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaPagoUtc { get; set; }
    }

    public sealed class ValoracionPublicDto
    {
        public Guid ValoracionGuid { get; set; }
        public decimal PuntuacionGeneral { get; set; }
        public decimal? PuntuacionLimpieza { get; set; }
        public decimal? PuntuacionConfort { get; set; }
        public decimal? PuntuacionUbicacion { get; set; }
        public decimal? PuntuacionInstalaciones { get; set; }
        public decimal? PuntuacionPersonal { get; set; }
        public decimal? PuntuacionCalidadPrecio { get; set; }
        public string? ComentarioPositivo { get; set; }
        public string? ComentarioNegativo { get; set; }
        public string? TipoViaje { get; set; }
        public string EstadoValoracion { get; set; } = string.Empty;
        public bool PublicadaEnPortal { get; set; }
        public string? RespuestaHotel { get; set; }
        public DateTime? FechaRespuestaUtc { get; set; }
        public DateTime FechaRegistroUtc { get; set; }
    }
}
