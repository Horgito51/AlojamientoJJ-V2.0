using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Servicio.Hotel.Business.DTOs.Alojamiento;
using Servicio.Hotel.Business.DTOs.Facturacion;
using Servicio.Hotel.Business.DTOs.Hospedaje;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.Business.DTOs.Seguridad;
using Servicio.Hotel.Business.DTOs.Valoraciones;

namespace Servicio.Hotel.API.Models.Requests.Internal
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class LogoutRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class CambiarPasswordRequest
    {
        public string PasswordActual { get; set; } = string.Empty;
        public string PasswordNuevo { get; set; } = string.Empty;
    }

    public class UsuarioCreateRequest
    {
        public int? IdCliente { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string EstadoUsuario { get; set; } = "ACT";
        public bool Activo { get; set; } = true;
        // Compatibilidad con clientes antiguos
        public int? IdRol { get; set; }

        // Contrato actual: IDs de roles
        public List<int> Roles { get; set; } = new();
    }

    public class UsuarioUpdateRequest
    {
        public string Correo { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string EstadoUsuario { get; set; } = "ACT";
        public bool Activo { get; set; } = true;
        public List<RolDTO> Roles { get; set; } = new();
    }

    public class InhabilitarRequest
    {
        public string Motivo { get; set; } = string.Empty;
    }

    public class RolUpsertRequest
    {
        public string NombreRol { get; set; } = string.Empty;
        public string DescripcionRol { get; set; } = string.Empty;
        public string EstadoRol { get; set; } = "ACT";
        public bool Activo { get; set; } = true;
    }

    public class RolPermisosUpsertRequest
    {
        public List<string> Permisos { get; set; } = new();
    }

    public class ClienteCreateRequest
    {
        public string TipoIdentificacion { get; set; } = string.Empty;
        public string NumeroIdentificacion { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? RazonSocial { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string Estado { get; set; } = "ACT";
    }

    public class ClienteUpdateRequest
    {
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? RazonSocial { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string Estado { get; set; } = "ACT";
    }

    public class ReservaCreateRequest
    {
        public int IdCliente { get; set; }
        public int IdSucursal { get; set; }
        public Guid? ClienteGuid { get; set; }
        public Guid? SucursalGuid { get; set; }
        public ClienteCreateRequest? Cliente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal SubtotalReserva { get; set; }
        public decimal ValorIva { get; set; }
        public decimal TotalReserva { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string OrigenCanalReserva { get; set; } = string.Empty;
        public string EstadoReserva { get; set; } = "PEN";
        public string? Observaciones { get; set; }
        public bool EsWalkin { get; set; }

        public List<ReservaHabitacionCreateRequest> Habitaciones { get; set; } = new();
    }

    public class ReservaHabitacionIdRequest
    {
        public int IdHabitacion { get; set; }
    }

    public class ReservaHabitacionCreateRequest
    {
        public int IdHabitacion { get; set; }
        public Guid? TipoHabitacionGuid { get; set; }
        public int NumHabitaciones { get; set; } = 1;
        public int? IdTarifa { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int NumAdultos { get; set; } = 1;
        public int NumNinos { get; set; } = 0;
        public decimal PrecioNocheAplicado { get; set; }
        public decimal SubtotalLinea { get; set; }
        public decimal ValorIvaLinea { get; set; }
        public decimal DescuentoLinea { get; set; } = 0;
        public decimal TotalLinea { get; set; }
        public string EstadoDetalle { get; set; } = "PEN";
    }

    public class ReservaUpdateRequest
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal SubtotalReserva { get; set; }
        public decimal ValorIva { get; set; }
        public decimal TotalReserva { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string EstadoReserva { get; set; } = "PEN";
        public string? Observaciones { get; set; }
    }

    public class CancelarReservaRequest
    {
        public string Motivo { get; set; } = string.Empty;
    }

    public class ReservaPrecioRequest
    {
        public int IdHabitacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? Canal { get; set; }
    }

    public class HabitacionCreateRequest
    {
        public int IdSucursal { get; set; }
        public int IdTipoHabitacion { get; set; }
        public string NumeroHabitacion { get; set; } = string.Empty;
        public int? Piso { get; set; }
        public int CapacidadHabitacion { get; set; }
        public decimal PrecioBase { get; set; }
        public string? Url { get; set; }
        public string? DescripcionHabitacion { get; set; }
        public string EstadoHabitacion { get; set; } = "DIS";
    }

    public class HabitacionUpdateRequest
    {
        public int IdSucursal { get; set; }
        public int IdTipoHabitacion { get; set; }
        public string NumeroHabitacion { get; set; } = string.Empty;
        public int? Piso { get; set; }
        public int CapacidadHabitacion { get; set; }
        public decimal PrecioBase { get; set; }
        public string? Url { get; set; }
        public string? DescripcionHabitacion { get; set; }
        public string EstadoHabitacion { get; set; } = "DIS";
    }

    public class HabitacionEstadoRequest
    {
        public string NuevoEstado { get; set; } = string.Empty;
    }

    public class TarifaUpsertRequest
    {
        public string CodigoTarifa { get; set; } = string.Empty;
        public int IdSucursal { get; set; }
        public int IdTipoHabitacion { get; set; }
        public string NombreTarifa { get; set; } = string.Empty;
        public string CanalTarifa { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal PrecioPorNoche { get; set; }
        public decimal PorcentajeIva { get; set; }
        public int MinNoches { get; set; }
        public int? MaxNoches { get; set; }
        public bool PermitePortalPublico { get; set; }
        public int Prioridad { get; set; }
        public string EstadoTarifa { get; set; } = "ACT";
    }

    public class TipoHabitacionUpsertRequest
    {
        public string CodigoTipoHabitacion { get; set; } = string.Empty;
        public string NombreTipoHabitacion { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int CapacidadAdultos { get; set; }
        public int CapacidadNinos { get; set; }
        public int CapacidadTotal { get; set; }
        public string TipoCama { get; set; } = string.Empty;
        public decimal? AreaM2 { get; set; }
        public bool PermiteEventos { get; set; }
        public bool PermiteReservaPublica { get; set; }
        public string EstadoTipoHabitacion { get; set; } = "ACT";
        public List<ImagenRequest>? Imagenes { get; set; }
    }

    public class SucursalUpsertRequest
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
        public List<ImagenRequest>? Imagenes { get; set; }
    }

    public class ImagenRequest
    {
        public Guid? ImagenGuid { get; set; }
        public string UrlImagen { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int Orden { get; set; }
        public bool EsPrincipal { get; set; }
    }

    public class SucursalPoliticasPatchRequest
    {
        public string? HoraCheckin { get; set; }
        public string? HoraCheckout { get; set; }
        public bool PermiteMascotas { get; set; }
        public bool SePermiteFumar { get; set; }
        public bool AceptaNinos { get; set; }
        public bool CheckinAnticipado { get; set; }
        public bool CheckoutTardio { get; set; }
    }

    public class CatalogoServicioUpsertRequest
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

    public class EstadiaCheckoutRequest
    {
        public string? Observaciones { get; set; }
        public bool RequiereMantenimiento { get; set; }
    }

    public class CargoEstadiaCreateRequest
    {
        public int? IdCatalogo { get; set; }
        public string DescripcionCargo { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal ValorIva { get; set; }
        public decimal TotalCargo { get; set; }
        public DateTime FechaConsumoUtc { get; set; }
        public string EstadoCargo { get; set; } = "PEN";
    }

    public class AnularFacturaRequest
    {
        public string Motivo { get; set; } = string.Empty;
    }

    public class PagoCreateRequest
    {
        public int IdFactura { get; set; }
        public int IdReserva { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public bool EsPagoElectronico { get; set; }
        public string? ProveedorPasarela { get; set; }
        public string? TransaccionExterna { get; set; }
        public string? CodigoAutorizacion { get; set; }
        public string? Referencia { get; set; }
        public string EstadoPago { get; set; } = "PEN";
        public DateTime FechaPagoUtc { get; set; }
        public string Moneda { get; set; } = "USD";
        public decimal TipoCambio { get; set; } = 1m;
        public string? RespuestaPasarela { get; set; }
    }

    public class PagoEstadoRequest
    {
        public string NuevoEstado { get; set; } = string.Empty;
    }

    public class PagoSimularRequest
    {
        public int IdReserva { get; set; }
        public decimal? Monto { get; set; }
        public string? TokenPago { get; set; }
        public string? Referencia { get; set; }
    }

    public class ValoracionCreateRequest
    {
        public int IdEstadia { get; set; }
        public int IdCliente { get; set; }
        public int IdSucursal { get; set; }
        public int? IdHabitacion { get; set; }
        public decimal? PuntuacionLimpieza { get; set; }
        public decimal? PuntuacionConfort { get; set; }
        public decimal? PuntuacionUbicacion { get; set; }
        public decimal? PuntuacionInstalaciones { get; set; }
        public decimal? PuntuacionPersonal { get; set; }
        public decimal? PuntuacionCalidadPrecio { get; set; }
        public string? ComentarioPositivo { get; set; }
        public string? ComentarioNegativo { get; set; }
        public string? TipoViaje { get; set; }
        public string EstadoValoracion { get; set; } = "PEN";
        public bool PublicadaEnPortal { get; set; }
    }

    public class ValoracionModeracionRequest
    {
        public string NuevoEstado { get; set; } = string.Empty;
        public string? Motivo { get; set; }
    }

    public class ValoracionRespuestaRequest
    {
        public string Respuesta { get; set; } = string.Empty;
    }

    public static class InternalRequestContractsMapper
    {
        public static Servicio.Hotel.Business.DTOs.Seguridad.UsuarioCreateDTO ToCreateDto(this UsuarioCreateRequest request)
            => new()
            {
                IdCliente = request.IdCliente,
                Username = request.Username,
                Correo = request.Correo,
                Password = request.Password,
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                EstadoUsuario = request.EstadoUsuario,
                Activo = request.Activo,
                IdRol = request.IdRol,
                Roles = (request.Roles != null && request.Roles.Count > 0)
                    ? request.Roles.Distinct().Select(id => new RolDTO { IdRol = id }).ToList()
                    : (request.IdRol.HasValue ? new List<RolDTO> { new RolDTO { IdRol = request.IdRol.Value } } : new List<RolDTO>())
            };

        public static Servicio.Hotel.Business.DTOs.Seguridad.UsuarioUpdateDTO ToUpdateDto(this UsuarioUpdateRequest request, int id)
            => new()
            {
                IdUsuario = id,
                Correo = request.Correo,
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                EstadoUsuario = request.EstadoUsuario,
                Activo = request.Activo,
                Roles = request.Roles
            };

        public static Servicio.Hotel.Business.DTOs.Seguridad.RolCreateDTO ToCreateDto(this RolUpsertRequest request)
            => new()
            {
                NombreRol = request.NombreRol,
                DescripcionRol = request.DescripcionRol,
                EstadoRol = request.EstadoRol,
                Activo = request.Activo
            };

        public static Servicio.Hotel.Business.DTOs.Seguridad.RolUpdateDTO ToUpdateDto(this RolUpsertRequest request, int id)
            => new()
            {
                IdRol = id,
                NombreRol = request.NombreRol,
                DescripcionRol = request.DescripcionRol,
                EstadoRol = request.EstadoRol,
                Activo = request.Activo
            };

        public static Servicio.Hotel.Business.DTOs.Reservas.ClienteCreateDTO ToCreateDto(this ClienteCreateRequest request)
            => new()
            {
                TipoIdentificacion = request.TipoIdentificacion,
                NumeroIdentificacion = request.NumeroIdentificacion,
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                RazonSocial = request.RazonSocial,
                Correo = request.Correo,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                Estado = request.Estado
            };

        public static Servicio.Hotel.Business.DTOs.Reservas.ClienteUpdateDTO ToUpdateDto(this ClienteUpdateRequest request, int id)
            => new()
            {
                IdCliente = id,
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                RazonSocial = request.RazonSocial,
                Correo = request.Correo,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                Estado = request.Estado
            };

        public static Servicio.Hotel.Business.DTOs.Reservas.ReservaCreateDTO ToCreateDto(this ReservaCreateRequest request)
            => new()
            {
                IdCliente = request.IdCliente,
                IdSucursal = request.IdSucursal,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                SubtotalReserva = request.SubtotalReserva,
                ValorIva = request.ValorIva,
                TotalReserva = request.TotalReserva,
                DescuentoAplicado = request.DescuentoAplicado,
                SaldoPendiente = request.SaldoPendiente,
                OrigenCanalReserva = request.OrigenCanalReserva,
                EstadoReserva = request.EstadoReserva,
                Observaciones = request.Observaciones ?? string.Empty,
                EsWalkin = request.EsWalkin,
                Habitaciones = request.Habitaciones.Select(h => new Servicio.Hotel.Business.DTOs.Reservas.ReservaHabitacionDTO
                {
                    IdHabitacion = h.IdHabitacion,
                    FechaInicio = h.FechaInicio,
                    FechaFin = h.FechaFin,
                    NumAdultos = h.NumAdultos,
                    NumNinos = h.NumNinos,
                    IdTarifa = h.IdTarifa,
                    PrecioNocheAplicado = h.PrecioNocheAplicado,
                    SubtotalLinea = h.SubtotalLinea,
                    ValorIvaLinea = h.ValorIvaLinea,
                    DescuentoLinea = h.DescuentoLinea,
                    TotalLinea = h.TotalLinea,
                    EstadoDetalle = h.EstadoDetalle
                }).ToList()
            };

        public static Servicio.Hotel.Business.DTOs.Reservas.ReservaUpdateDTO ToUpdateDto(this ReservaUpdateRequest request, int id)
            => new()
            {
                IdReserva = id,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                SubtotalReserva = request.SubtotalReserva,
                ValorIva = request.ValorIva,
                TotalReserva = request.TotalReserva,
                DescuentoAplicado = request.DescuentoAplicado,
                SaldoPendiente = request.SaldoPendiente,
                EstadoReserva = request.EstadoReserva,
                Observaciones = request.Observaciones ?? string.Empty
            };

        public static Servicio.Hotel.Business.DTOs.Alojamiento.HabitacionCreateDTO ToCreateDto(this HabitacionCreateRequest request)
            => new()
            {
                IdSucursal = request.IdSucursal,
                IdTipoHabitacion = request.IdTipoHabitacion,
                NumeroHabitacion = request.NumeroHabitacion,
                Piso = request.Piso,
                CapacidadHabitacion = request.CapacidadHabitacion,
                PrecioBase = request.PrecioBase,
                Url = request.Url ?? string.Empty,
                DescripcionHabitacion = request.DescripcionHabitacion ?? string.Empty,
                EstadoHabitacion = request.EstadoHabitacion
            };

        public static Servicio.Hotel.Business.DTOs.Alojamiento.HabitacionUpdateDTO ToUpdateDto(this HabitacionUpdateRequest request, int id)
            => new()
            {
                IdHabitacion = id,
                IdSucursal = request.IdSucursal,
                IdTipoHabitacion = request.IdTipoHabitacion,
                NumeroHabitacion = request.NumeroHabitacion,
                Piso = request.Piso,
                CapacidadHabitacion = request.CapacidadHabitacion,
                PrecioBase = request.PrecioBase,
                Url = request.Url ?? string.Empty,
                DescripcionHabitacion = request.DescripcionHabitacion ?? string.Empty,
                EstadoHabitacion = request.EstadoHabitacion
            };

        public static Servicio.Hotel.Business.DTOs.Alojamiento.TarifaCreateDTO ToCreateDto(this TarifaUpsertRequest request)
            => new()
            {
                CodigoTarifa = request.CodigoTarifa,
                IdSucursal = request.IdSucursal,
                IdTipoHabitacion = request.IdTipoHabitacion,
                NombreTarifa = request.NombreTarifa,
                CanalTarifa = request.CanalTarifa,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                PrecioPorNoche = request.PrecioPorNoche,
                PorcentajeIva = request.PorcentajeIva,
                MinNoches = request.MinNoches,
                MaxNoches = request.MaxNoches,
                PermitePortalPublico = request.PermitePortalPublico,
                Prioridad = request.Prioridad,
                EstadoTarifa = request.EstadoTarifa
            };

        public static Servicio.Hotel.Business.DTOs.Alojamiento.TarifaUpdateDTO ToUpdateDto(this TarifaUpsertRequest request, int id)
            => new()
            {
                IdTarifa = id,
                CodigoTarifa = request.CodigoTarifa,
                IdSucursal = request.IdSucursal,
                IdTipoHabitacion = request.IdTipoHabitacion,
                NombreTarifa = request.NombreTarifa,
                CanalTarifa = request.CanalTarifa,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                PrecioPorNoche = request.PrecioPorNoche,
                PorcentajeIva = request.PorcentajeIva,
                MinNoches = request.MinNoches,
                MaxNoches = request.MaxNoches,
                PermitePortalPublico = request.PermitePortalPublico,
                Prioridad = request.Prioridad,
                EstadoTarifa = request.EstadoTarifa
            };

        public static Servicio.Hotel.Business.DTOs.Alojamiento.TipoHabitacionCreateDTO ToCreateDto(this TipoHabitacionUpsertRequest request)
            => new()
            {
                CodigoTipoHabitacion = request.CodigoTipoHabitacion,
                NombreTipoHabitacion = request.NombreTipoHabitacion,
                Descripcion = request.Descripcion,
                CapacidadAdultos = request.CapacidadAdultos,
                CapacidadNinos = request.CapacidadNinos,
                CapacidadTotal = request.CapacidadTotal,
                TipoCama = request.TipoCama,
                AreaM2 = request.AreaM2,
                PermiteEventos = request.PermiteEventos,
                PermiteReservaPublica = request.PermiteReservaPublica,
                EstadoTipoHabitacion = request.EstadoTipoHabitacion,
                Imagenes = request.Imagenes?.Select(i => new ImagenDTO
                {
                    ImagenGuid = i.ImagenGuid ?? Guid.Empty,
                    UrlImagen = i.UrlImagen,
                    Descripcion = i.Descripcion,
                    Orden = i.Orden,
                    EsPrincipal = i.EsPrincipal
                }).ToList()
            };

        public static Servicio.Hotel.Business.DTOs.Alojamiento.TipoHabitacionUpdateDTO ToUpdateDto(this TipoHabitacionUpsertRequest request, int id)
            => new()
            {
                IdTipoHabitacion = id,
                CodigoTipoHabitacion = request.CodigoTipoHabitacion,
                NombreTipoHabitacion = request.NombreTipoHabitacion,
                Descripcion = request.Descripcion,
                CapacidadAdultos = request.CapacidadAdultos,
                CapacidadNinos = request.CapacidadNinos,
                CapacidadTotal = request.CapacidadTotal,
                TipoCama = request.TipoCama,
                AreaM2 = request.AreaM2,
                PermiteEventos = request.PermiteEventos,
                PermiteReservaPublica = request.PermiteReservaPublica,
                EstadoTipoHabitacion = request.EstadoTipoHabitacion,
                Imagenes = request.Imagenes?.Select(i => new ImagenDTO
                {
                    ImagenGuid = i.ImagenGuid ?? Guid.Empty,
                    UrlImagen = i.UrlImagen,
                    Descripcion = i.Descripcion,
                    Orden = i.Orden,
                    EsPrincipal = i.EsPrincipal
                }).ToList()
            };

        public static Servicio.Hotel.Business.DTOs.Alojamiento.SucursalCreateDTO ToCreateDto(this SucursalUpsertRequest request)
            => new()
            {
                CodigoSucursal = request.CodigoSucursal,
                NombreSucursal = request.NombreSucursal,
                DescripcionSucursal = request.DescripcionSucursal,
                DescripcionCorta = request.DescripcionCorta,
                TipoAlojamiento = request.TipoAlojamiento,
                Estrellas = request.Estrellas,
                CategoriaViaje = request.CategoriaViaje,
                Pais = request.Pais,
                Provincia = request.Provincia,
                Ciudad = request.Ciudad,
                Ubicacion = request.Ubicacion,
                Direccion = request.Direccion,
                CodigoPostal = request.CodigoPostal,
                Telefono = request.Telefono,
                Correo = request.Correo,
                Latitud = request.Latitud,
                Longitud = request.Longitud,
                HoraCheckin = request.HoraCheckin,
                HoraCheckout = request.HoraCheckout,
                CheckinAnticipado = request.CheckinAnticipado,
                CheckoutTardio = request.CheckoutTardio,
                AceptaNinos = request.AceptaNinos,
                EdadMinimaHuesped = request.EdadMinimaHuesped,
                PermiteMascotas = request.PermiteMascotas,
                SePermiteFumar = request.SePermiteFumar,
                EstadoSucursal = request.EstadoSucursal,
                Imagenes = request.Imagenes?.Select(i => new ImagenDTO
                {
                    ImagenGuid = i.ImagenGuid ?? Guid.Empty,
                    UrlImagen = i.UrlImagen,
                    Descripcion = i.Descripcion,
                    Orden = i.Orden,
                    EsPrincipal = i.EsPrincipal
                }).ToList()
            };

        public static Servicio.Hotel.Business.DTOs.Alojamiento.SucursalPoliticasUpdateDTO ToDto(this SucursalPoliticasPatchRequest request)
            => new()
            {
                HoraCheckin = request.HoraCheckin,
                HoraCheckout = request.HoraCheckout,
                PermiteMascotas = request.PermiteMascotas,
                SePermiteFumar = request.SePermiteFumar,
                AceptaNinos = request.AceptaNinos,
                CheckinAnticipado = request.CheckinAnticipado,
                CheckoutTardio = request.CheckoutTardio
            };

        public static Servicio.Hotel.Business.DTOs.Alojamiento.SucursalUpdateDTO ToUpdateDto(this SucursalUpsertRequest request, int id)
            => new()
            {
                IdSucursal = id,
                CodigoSucursal = request.CodigoSucursal,
                NombreSucursal = request.NombreSucursal,
                DescripcionSucursal = request.DescripcionSucursal,
                DescripcionCorta = request.DescripcionCorta,
                TipoAlojamiento = request.TipoAlojamiento,
                Estrellas = request.Estrellas,
                CategoriaViaje = request.CategoriaViaje,
                Pais = request.Pais,
                Provincia = request.Provincia,
                Ciudad = request.Ciudad,
                Ubicacion = request.Ubicacion,
                Direccion = request.Direccion,
                CodigoPostal = request.CodigoPostal,
                Telefono = request.Telefono,
                Correo = request.Correo,
                Latitud = request.Latitud,
                Longitud = request.Longitud,
                HoraCheckin = request.HoraCheckin,
                HoraCheckout = request.HoraCheckout,
                CheckinAnticipado = request.CheckinAnticipado,
                CheckoutTardio = request.CheckoutTardio,
                AceptaNinos = request.AceptaNinos,
                EdadMinimaHuesped = request.EdadMinimaHuesped,
                PermiteMascotas = request.PermiteMascotas,
                SePermiteFumar = request.SePermiteFumar,
                EstadoSucursal = request.EstadoSucursal,
                Imagenes = request.Imagenes?.Select(i => new ImagenDTO
                {
                    ImagenGuid = i.ImagenGuid ?? Guid.Empty,
                    UrlImagen = i.UrlImagen,
                    Descripcion = i.Descripcion,
                    Orden = i.Orden,
                    EsPrincipal = i.EsPrincipal
                }).ToList()
            };

        public static Servicio.Hotel.Business.DTOs.Alojamiento.CatalogoServicioCreateDTO ToCreateDto(this CatalogoServicioUpsertRequest request)
            => new()
            {
                IdSucursal = request.IdSucursal,
                CodigoCatalogo = request.CodigoCatalogo,
                NombreCatalogo = request.NombreCatalogo,
                TipoCatalogo = request.TipoCatalogo,
                CategoriaCatalogo = request.CategoriaCatalogo,
                DescripcionCatalogo = request.DescripcionCatalogo,
                PrecioBase = request.PrecioBase,
                AplicaIva = request.AplicaIva,
                Disponible24h = request.Disponible24h,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                IconoUrl = request.IconoUrl,
                EstadoCatalogo = request.EstadoCatalogo
            };

        public static Servicio.Hotel.Business.DTOs.Alojamiento.CatalogoServicioUpdateDTO ToUpdateDto(this CatalogoServicioUpsertRequest request, int id)
            => new()
            {
                IdCatalogo = id,
                IdSucursal = request.IdSucursal,
                CodigoCatalogo = request.CodigoCatalogo,
                NombreCatalogo = request.NombreCatalogo,
                TipoCatalogo = request.TipoCatalogo,
                CategoriaCatalogo = request.CategoriaCatalogo,
                DescripcionCatalogo = request.DescripcionCatalogo,
                PrecioBase = request.PrecioBase,
                AplicaIva = request.AplicaIva,
                Disponible24h = request.Disponible24h,
                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,
                IconoUrl = request.IconoUrl,
                EstadoCatalogo = request.EstadoCatalogo
            };

        public static CargoEstadiaDTO ToDto(this CargoEstadiaCreateRequest request, int estadiaId)
            => new()
            {
                IdEstadia = estadiaId,
                IdCatalogo = request.IdCatalogo,
                DescripcionCargo = request.DescripcionCargo,
                Cantidad = request.Cantidad,
                PrecioUnitario = request.PrecioUnitario,
                Subtotal = request.Subtotal,
                ValorIva = request.ValorIva,
                TotalCargo = request.TotalCargo,
                FechaConsumoUtc = request.FechaConsumoUtc,
                EstadoCargo = request.EstadoCargo
            };

        public static PagoDTO ToDto(this PagoCreateRequest request)
            => new()
            {
                IdFactura = request.IdFactura,
                IdReserva = request.IdReserva,
                Monto = request.Monto,
                MetodoPago = request.MetodoPago,
                EsPagoElectronico = request.EsPagoElectronico,
                ProveedorPasarela = request.ProveedorPasarela ?? string.Empty,
                TransaccionExterna = request.TransaccionExterna ?? string.Empty,
                CodigoAutorizacion = request.CodigoAutorizacion ?? string.Empty,
                Referencia = request.Referencia ?? string.Empty,
                EstadoPago = request.EstadoPago,
                FechaPagoUtc = request.FechaPagoUtc,
                Moneda = request.Moneda,
                TipoCambio = request.TipoCambio,
                RespuestaPasarela = request.RespuestaPasarela ?? string.Empty
            };

        public static ValoracionDTO ToDto(this ValoracionCreateRequest request)
            => new()
            {
                IdEstadia = request.IdEstadia,
                IdCliente = request.IdCliente,
                IdSucursal = request.IdSucursal,
                IdHabitacion = request.IdHabitacion,
                PuntuacionLimpieza = request.PuntuacionLimpieza,
                PuntuacionConfort = request.PuntuacionConfort,
                PuntuacionUbicacion = request.PuntuacionUbicacion,
                PuntuacionInstalaciones = request.PuntuacionInstalaciones,
                PuntuacionPersonal = request.PuntuacionPersonal,
                PuntuacionCalidadPrecio = request.PuntuacionCalidadPrecio,
                ComentarioPositivo = request.ComentarioPositivo ?? string.Empty,
                ComentarioNegativo = request.ComentarioNegativo ?? string.Empty,
                TipoViaje = request.TipoViaje ?? string.Empty,
                EstadoValoracion = request.EstadoValoracion,
                PublicadaEnPortal = request.PublicadaEnPortal
            };
    }
}
