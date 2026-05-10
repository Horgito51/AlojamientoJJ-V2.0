using System;
using System.Collections.Generic;

namespace Servicio.Hotel.Business.DTOs.Booking
{
    public sealed class BookingPagedResponseDTO<T>
    {
        public List<T> Items { get; set; } = new();
        public int Pagina { get; set; }
        public int Limite { get; set; }
        public int TotalResultados { get; set; }
        public int TotalPaginas { get; set; }
        public bool TieneSiguiente { get; set; }
        public bool TieneAnterior { get; set; }
    }

    public sealed class AccommodationSearchQueryDTO
    {
        public string? Destino { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public int? NumAdultos { get; set; }
        public int? NumNinos { get; set; }
        public int? NumHabitaciones { get; set; }
        public string? TipoAlojamiento { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public string? CategoriaViaje { get; set; }
        public string? OrdenarPor { get; set; }
        public int Pagina { get; set; } = 1;
        public int Limite { get; set; } = 20;
    }

    public class AccommodationSearchItemDTO
    {
        public Guid SucursalGuid { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Ciudad { get; set; }
        public string? Provincia { get; set; }
        public string? Pais { get; set; }
        public string? Direccion { get; set; }
        public string? Descripcion { get; set; }
        public string? Categoria { get; set; }
        public int? Estrellas { get; set; }
        public string? TipoAlojamiento { get; set; }
        public decimal? PrecioDesde { get; set; }
        public string Moneda { get; set; } = "USD";
        public string? ImagenPrincipalUrl { get; set; }
        public decimal? PromedioValoracion { get; set; }
        public int TotalValoraciones { get; set; }
        public int HabitacionesDisponibles { get; set; }
        public List<string> ServiciosDestacados { get; set; } = new();
        public string? HoraCheckIn { get; set; }
        public string? HoraCheckOut { get; set; }
        public bool AceptaNinos { get; set; }
        public bool PermiteMascotas { get; set; }
    }

    public sealed class AccommodationDetailResponseDTO : AccommodationSearchItemDTO
    {
        public string? DescripcionCompleta { get; set; }
        public List<AccommodationRoomTypeDTO> TiposHabitacion { get; set; } = new();
        public List<AccommodationTariffDTO> TarifasActivas { get; set; } = new();
        public List<string> Amenities { get; set; } = new();
        public List<string> Imagenes { get; set; } = new();
        public AccommodationPolicyDTO Politicas { get; set; } = new();
        public AccommodationAvailabilityDTO Disponibilidad { get; set; } = new();
    }

    public sealed class AccommodationRoomTypeDTO
    {
        public Guid TipoHabitacionGuid { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? TipoCama { get; set; }
        public int CapacidadAdultos { get; set; }
        public int CapacidadNinos { get; set; }
        public decimal? AreaM2 { get; set; }
        public decimal PrecioBase { get; set; }
        public List<string> Imagenes { get; set; } = new();
        public int? DisponiblesEnRango { get; set; }
    }

    public sealed class AccommodationTariffDTO
    {
        public Guid TarifaGuid { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioPorNoche { get; set; }
        public string Moneda { get; set; } = "USD";
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? MinNoches { get; set; }
        public Guid? TipoHabitacionGuid { get; set; }
    }

    public sealed class AccommodationPolicyDTO
    {
        public string? HoraCheckIn { get; set; }
        public string? HoraCheckOut { get; set; }
        public bool AceptaNinos { get; set; }
        public bool PermiteMascotas { get; set; }
        public string? Politicas { get; set; }
    }

    public sealed class AccommodationAvailabilityDTO
    {
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public List<AvailabilityByRoomTypeDTO> PorTipoHabitacion { get; set; } = new();
    }

    public sealed class AvailabilityByRoomTypeDTO
    {
        public Guid TipoHabitacionGuid { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Disponibles { get; set; }
    }

    public sealed class AccommodationReviewDTO
    {
        public Guid ValoracionGuid { get; set; }
        public decimal Puntuacion { get; set; }
        public string? ComentarioPositivo { get; set; }
        public string? ComentarioNegativo { get; set; }
        public string? TipoViaje { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreVisibleCliente { get; set; } = string.Empty;
        public string? RespuestaPropiedad { get; set; }
    }

    public sealed class HabitacionPublicListItemDTO
    {
        public Guid HabitacionGuid { get; set; }
        public Guid TipoHabitacionGuid { get; set; }
        public string TipoNombre { get; set; } = string.Empty;
        public string NumeroHabitacion { get; set; } = string.Empty;
        public int? Piso { get; set; }
        public int CapacidadAdultos { get; set; }
        public int CapacidadNinos { get; set; }
        public decimal PrecioBase { get; set; }
        public string Moneda { get; set; } = "USD";
        public string EstadoHabitacion { get; set; } = string.Empty;
        public bool? DisponibleEnRango { get; set; }
    }
}
