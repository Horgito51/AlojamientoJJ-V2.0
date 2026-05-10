using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Servicio.Hotel.Business.DTOs.Booking;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using Servicio.Hotel.Business.Interfaces.Booking;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.Business.Services.Booking
{
    public sealed class BookingAccommodationService : IBookingAccommodationService
    {
        private static readonly string[] ActiveReservationStates = { "PEN", "CON" };
        private readonly ServicioHotelDbContext _context;
        private readonly ISucursalService _sucursalService;
        private readonly IHabitacionService _habitacionService;

        public BookingAccommodationService(
            ServicioHotelDbContext context,
            ISucursalService sucursalService,
            IHabitacionService habitacionService)
        {
            _context = context;
            _sucursalService = sucursalService;
            _habitacionService = habitacionService;
        }

        public async Task<BookingPagedResponseDTO<AccommodationSearchItemDTO>> SearchAsync(AccommodationSearchQueryDTO query, CancellationToken ct = default)
        {
            NormalizeSearchQuery(query);

            var sucursales = await _context.Sucursales
                .AsNoTracking()
                .Where(s => s.EstadoSucursal == "ACT")
                .ToListAsync(ct);

            sucursales = ApplySucursalFilters(sucursales, query);
            var cards = new List<AccommodationSearchItemDTO>();

            foreach (var sucursal in sucursales)
            {
                var card = await BuildSearchItemAsync(sucursal, query.FechaEntrada, query.FechaSalida, ct);

                if (query.NumHabitaciones.HasValue && card.HabitacionesDisponibles < query.NumHabitaciones.Value)
                    continue;
                if (query.PrecioMin.HasValue && (!card.PrecioDesde.HasValue || card.PrecioDesde.Value < query.PrecioMin.Value))
                    continue;
                if (query.PrecioMax.HasValue && (!card.PrecioDesde.HasValue || card.PrecioDesde.Value > query.PrecioMax.Value))
                    continue;

                cards.Add(card);
            }

            cards = ApplyCapacityFilters(cards, query);
            cards = ApplyOrdering(cards, query.OrdenarPor);

            return Page(cards, query.Pagina, query.Limite);
        }

        public async Task<AccommodationDetailResponseDTO> GetDetailAsync(Guid sucursalGuid, DateTime? fechaEntrada, DateTime? fechaSalida, CancellationToken ct = default)
        {
            if (sucursalGuid == Guid.Empty)
                throw new ValidationException("BOOK-SUC-001", "sucursalGuid es obligatorio.");

            ValidateOptionalDateRange(fechaEntrada, fechaSalida);

            var sucursal = await _context.Sucursales
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SucursalGuid == sucursalGuid && s.EstadoSucursal == "ACT", ct);

            if (sucursal == null)
                throw new NotFoundException("BOOK-SUC-404", $"No se encontro la sucursal con GUID {sucursalGuid}.");

            var searchItem = await BuildSearchItemAsync(sucursal, fechaEntrada, fechaSalida, ct);
            var tipos = await GetTiposBySucursalAsync(sucursal.IdSucursal, ct);
            var disponibilidad = await BuildAvailabilityAsync(sucursal.IdSucursal, tipos, fechaEntrada, fechaSalida, ct);
            var imagenesPorTipo = await GetImagenesPorTipoAsync(tipos.Select(t => t.IdTipoHabitacion), ct);
            var tarifas = await GetTarifasActivasAsync(sucursal.IdSucursal, tipos, ct);
            var amenities = await GetAmenitiesAsync(sucursal.IdSucursal, tipos.Select(t => t.IdTipoHabitacion), ct);
            var preciosPorTipo = await GetPreciosBasePorTipoAsync(sucursal.IdSucursal, ct);

            return new AccommodationDetailResponseDTO
            {
                SucursalGuid = searchItem.SucursalGuid,
                Nombre = searchItem.Nombre,
                Ciudad = searchItem.Ciudad,
                Provincia = searchItem.Provincia,
                Pais = searchItem.Pais,
                Direccion = searchItem.Direccion,
                Descripcion = searchItem.Descripcion,
                Categoria = searchItem.Categoria,
                Estrellas = searchItem.Estrellas,
                TipoAlojamiento = searchItem.TipoAlojamiento,
                PrecioDesde = searchItem.PrecioDesde,
                Moneda = searchItem.Moneda,
                ImagenPrincipalUrl = searchItem.ImagenPrincipalUrl,
                PromedioValoracion = searchItem.PromedioValoracion,
                TotalValoraciones = searchItem.TotalValoraciones,
                HabitacionesDisponibles = searchItem.HabitacionesDisponibles,
                ServiciosDestacados = searchItem.ServiciosDestacados,
                HoraCheckIn = searchItem.HoraCheckIn,
                HoraCheckOut = searchItem.HoraCheckOut,
                AceptaNinos = searchItem.AceptaNinos,
                PermiteMascotas = searchItem.PermiteMascotas,
                DescripcionCompleta = FirstNonEmpty(sucursal.DescripcionSucursal, sucursal.DescripcionCorta),
                TiposHabitacion = tipos.Select(t => new AccommodationRoomTypeDTO
                {
                    TipoHabitacionGuid = t.TipoHabitacionGuid,
                    Nombre = t.NombreTipoHabitacion,
                    TipoCama = t.TipoCama,
                    CapacidadAdultos = t.CapacidadAdultos,
                    CapacidadNinos = t.CapacidadNinos,
                    AreaM2 = t.AreaM2,
                    PrecioBase = preciosPorTipo.TryGetValue(t.IdTipoHabitacion, out var precio) ? precio : 0m,
                    Imagenes = imagenesPorTipo.TryGetValue(t.IdTipoHabitacion, out var imgs) ? imgs : new List<string>(),
                    DisponiblesEnRango = disponibilidad.PorTipoHabitacion.FirstOrDefault(d => d.TipoHabitacionGuid == t.TipoHabitacionGuid)?.Disponibles
                }).ToList(),
                TarifasActivas = tarifas,
                Amenities = amenities,
                Imagenes = imagenesPorTipo.SelectMany(i => i.Value).Distinct().ToList(),
                Politicas = new AccommodationPolicyDTO
                {
                    HoraCheckIn = sucursal.HoraCheckin,
                    HoraCheckOut = sucursal.HoraCheckout,
                    AceptaNinos = sucursal.AceptaNinos,
                    PermiteMascotas = sucursal.PermiteMascotas,
                    Politicas = BuildPolicyText(sucursal)
                },
                Disponibilidad = disponibilidad
            };
        }

        public async Task<BookingPagedResponseDTO<AccommodationReviewDTO>> GetReviewsAsync(Guid sucursalGuid, int pagina, int limite, CancellationToken ct = default)
        {
            if (sucursalGuid == Guid.Empty)
                throw new ValidationException("BOOK-REV-001", "sucursalGuid es obligatorio.");

            var sucursal = await _sucursalService.GetByGuidAsync(sucursalGuid, ct);
            pagina = Math.Max(1, pagina);
            limite = Math.Clamp(limite, 1, 50);

            var query = from valoracion in _context.Valoraciones.AsNoTracking()
                        join cliente in _context.Clientes.AsNoTracking() on valoracion.IdCliente equals cliente.IdCliente
                        where valoracion.IdSucursal == sucursal.IdSucursal
                            && valoracion.EstadoValoracion == "APR"
                            && valoracion.PublicadaEnPortal
                        orderby valoracion.FechaRegistroUtc descending
                        select new AccommodationReviewDTO
                        {
                            ValoracionGuid = valoracion.ValoracionGuid,
                            Puntuacion = valoracion.PuntuacionGeneral,
                            ComentarioPositivo = valoracion.ComentarioPositivo,
                            ComentarioNegativo = valoracion.ComentarioNegativo,
                            TipoViaje = valoracion.TipoViaje,
                            Fecha = valoracion.FechaRegistroUtc,
                            NombreVisibleCliente = (cliente.Nombres + " " + (cliente.Apellidos ?? string.Empty)).Trim(),
                            RespuestaPropiedad = valoracion.RespuestaHotel
                        };

            var total = await query.CountAsync(ct);
            var items = await query.Skip((pagina - 1) * limite).Take(limite).ToListAsync(ct);
            return BuildPaged(items, pagina, limite, total);
        }

        public async Task<IEnumerable<HabitacionPublicListItemDTO>> GetHabitacionesDisponiblesAsync(Guid sucursalGuid, DateTime fechaEntrada, DateTime fechaSalida, CancellationToken ct = default)
        {
            if (sucursalGuid == Guid.Empty)
                throw new ValidationException("BOOK-HAB-001", "sucursalGuid es obligatorio.");
            ValidateRequiredDateRange(fechaEntrada, fechaSalida);

            var sucursal = await _sucursalService.GetByGuidAsync(sucursalGuid, ct);
            var habitaciones = await _habitacionService.GetDisponiblesAsync(sucursal.IdSucursal, fechaEntrada, fechaSalida, ct);
            var tipos = await _context.TiposHabitacion.AsNoTracking().ToDictionaryAsync(t => t.IdTipoHabitacion, ct);

            return habitaciones
                .Where(h => h.EstadoHabitacion == "DIS")
                .Select(h =>
                {
                    var tipo = tipos[h.IdTipoHabitacion];
                    return new HabitacionPublicListItemDTO
                    {
                        HabitacionGuid = h.HabitacionGuid,
                        TipoHabitacionGuid = tipo.TipoHabitacionGuid,
                        TipoNombre = tipo.NombreTipoHabitacion,
                        NumeroHabitacion = h.NumeroHabitacion,
                        Piso = h.Piso,
                        CapacidadAdultos = tipo.CapacidadAdultos,
                        CapacidadNinos = tipo.CapacidadNinos,
                        PrecioBase = h.PrecioBase,
                        EstadoHabitacion = h.EstadoHabitacion,
                        DisponibleEnRango = true
                    };
                })
                .OrderBy(h => h.TipoNombre)
                .ThenBy(h => h.NumeroHabitacion)
                .ToList();
        }

        private static void NormalizeSearchQuery(AccommodationSearchQueryDTO query)
        {
            query.Pagina = Math.Max(1, query.Pagina);
            query.Limite = Math.Clamp(query.Limite, 1, 50);

            ValidateOptionalDateRange(query.FechaEntrada, query.FechaSalida);

            if (query.NumAdultos is < 0 || query.NumNinos is < 0 || query.NumHabitaciones is < 0)
                throw new ValidationException("BOOK-SEARCH-001", "Los filtros de capacidad no pueden ser negativos.");

            if (query.PrecioMin.HasValue && query.PrecioMax.HasValue && query.PrecioMax < query.PrecioMin)
                throw new ValidationException("BOOK-SEARCH-002", "precioMax debe ser mayor o igual a precioMin.");
        }

        private static void ValidateOptionalDateRange(DateTime? fechaEntrada, DateTime? fechaSalida)
        {
            if (fechaEntrada.HasValue != fechaSalida.HasValue)
                throw new ValidationException("BOOK-DATE-001", "fechaEntrada y fechaSalida deben enviarse juntas.");

            if (fechaEntrada.HasValue && fechaSalida <= fechaEntrada)
                throw new ValidationException("BOOK-DATE-002", "fechaSalida debe ser posterior a fechaEntrada.");
        }

        private static void ValidateRequiredDateRange(DateTime fechaEntrada, DateTime fechaSalida)
        {
            if (fechaEntrada == default)
                throw new ValidationException("BOOK-DATE-003", "fechaEntrada es obligatoria.");
            if (fechaSalida == default)
                throw new ValidationException("BOOK-DATE-004", "fechaSalida es obligatoria.");
            if (fechaSalida <= fechaEntrada)
                throw new ValidationException("BOOK-DATE-002", "fechaSalida debe ser posterior a fechaEntrada.");
        }

        private static List<SucursalEntity> ApplySucursalFilters(List<SucursalEntity> sucursales, AccommodationSearchQueryDTO query)
        {
            if (!string.IsNullOrWhiteSpace(query.Destino))
            {
                var destino = query.Destino.Trim();
                sucursales = sucursales.Where(s =>
                    Contains(s.Ciudad, destino) ||
                    Contains(s.Provincia, destino) ||
                    Contains(s.Pais, destino) ||
                    Contains(s.NombreSucursal, destino) ||
                    Contains(s.Direccion, destino)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(query.TipoAlojamiento))
                sucursales = sucursales.Where(s => Contains(s.TipoAlojamiento, query.TipoAlojamiento)).ToList();

            if (!string.IsNullOrWhiteSpace(query.CategoriaViaje))
                sucursales = sucursales.Where(s => Contains(s.CategoriaViaje, query.CategoriaViaje)).ToList();

            return sucursales;
        }

        private static List<AccommodationSearchItemDTO> ApplyCapacityFilters(List<AccommodationSearchItemDTO> cards, AccommodationSearchQueryDTO query)
        {
            if (!query.NumAdultos.HasValue && !query.NumNinos.HasValue)
                return cards;

            return cards.Where(c => c.HabitacionesDisponibles > 0).ToList();
        }

        private static List<AccommodationSearchItemDTO> ApplyOrdering(List<AccommodationSearchItemDTO> cards, string? ordenarPor)
        {
            return (ordenarPor ?? string.Empty).Trim().ToLowerInvariant() switch
            {
                "precio_desc" or "precio-desc" => cards.OrderByDescending(c => c.PrecioDesde ?? decimal.MaxValue).ToList(),
                "valoracion" or "rating" => cards.OrderByDescending(c => c.PromedioValoracion ?? 0m).ThenBy(c => c.PrecioDesde ?? decimal.MaxValue).ToList(),
                "nombre" => cards.OrderBy(c => c.Nombre).ToList(),
                _ => cards.OrderBy(c => c.PrecioDesde ?? decimal.MaxValue).ThenByDescending(c => c.PromedioValoracion ?? 0m).ToList()
            };
        }

        private async Task<AccommodationSearchItemDTO> BuildSearchItemAsync(SucursalEntity sucursal, DateTime? fechaEntrada, DateTime? fechaSalida, CancellationToken ct)
        {
            var habitacionesQuery = _context.Habitaciones
                .AsNoTracking()
                .Where(h => h.IdSucursal == sucursal.IdSucursal && h.EstadoHabitacion == "DIS");

            if (fechaEntrada.HasValue && fechaSalida.HasValue)
            {
                var ocupadas = await GetHabitacionesOcupadasAsync(sucursal.IdSucursal, fechaEntrada.Value, fechaSalida.Value, ct);
                habitacionesQuery = habitacionesQuery.Where(h => !ocupadas.Contains(h.IdHabitacion));
            }

            var habitaciones = await habitacionesQuery.ToListAsync(ct);
            var tipoIds = habitaciones.Select(h => h.IdTipoHabitacion).Distinct().ToList();
            var imagenPrincipal = await GetImagenPrincipalAsync(tipoIds, ct);
            var servicios = await GetAmenitiesAsync(sucursal.IdSucursal, tipoIds, ct);
            var rating = await _context.Valoraciones
                .AsNoTracking()
                .Where(v => v.IdSucursal == sucursal.IdSucursal && v.EstadoValoracion == "APR" && v.PublicadaEnPortal)
                .GroupBy(v => v.IdSucursal)
                .Select(g => new { Promedio = g.Average(v => v.PuntuacionGeneral), Total = g.Count() })
                .FirstOrDefaultAsync(ct);

            return new AccommodationSearchItemDTO
            {
                SucursalGuid = sucursal.SucursalGuid,
                Nombre = sucursal.NombreSucursal,
                Ciudad = sucursal.Ciudad,
                Provincia = sucursal.Provincia,
                Pais = sucursal.Pais,
                Direccion = sucursal.Direccion,
                Descripcion = FirstNonEmpty(sucursal.DescripcionCorta, sucursal.DescripcionSucursal),
                Categoria = sucursal.CategoriaViaje,
                Estrellas = sucursal.Estrellas,
                TipoAlojamiento = sucursal.TipoAlojamiento,
                PrecioDesde = habitaciones.Count == 0 ? null : habitaciones.Min(h => h.PrecioBase),
                ImagenPrincipalUrl = imagenPrincipal,
                PromedioValoracion = rating == null ? null : Math.Round(rating.Promedio, 1),
                TotalValoraciones = rating?.Total ?? 0,
                HabitacionesDisponibles = habitaciones.Count,
                ServiciosDestacados = servicios.Take(6).ToList(),
                HoraCheckIn = sucursal.HoraCheckin,
                HoraCheckOut = sucursal.HoraCheckout,
                AceptaNinos = sucursal.AceptaNinos,
                PermiteMascotas = sucursal.PermiteMascotas
            };
        }

        private async Task<List<TipoHabitacionEntity>> GetTiposBySucursalAsync(int idSucursal, CancellationToken ct)
        {
            return await _context.Habitaciones
                .AsNoTracking()
                .Where(h => h.IdSucursal == idSucursal)
                .Select(h => h.IdTipoHabitacion)
                .Distinct()
                .Join(_context.TiposHabitacion.AsNoTracking().Where(t => t.EstadoTipoHabitacion == "ACT" && t.PermiteReservaPublica),
                    id => id,
                    tipo => tipo.IdTipoHabitacion,
                    (_, tipo) => tipo)
                .OrderBy(t => t.NombreTipoHabitacion)
                .ToListAsync(ct);
        }

        private async Task<AccommodationAvailabilityDTO> BuildAvailabilityAsync(int idSucursal, List<TipoHabitacionEntity> tipos, DateTime? fechaEntrada, DateTime? fechaSalida, CancellationToken ct)
        {
            var start = fechaEntrada ?? DateTime.UtcNow.Date;
            var end = fechaSalida ?? start.AddDays(1);
            var ocupadas = await GetHabitacionesOcupadasAsync(idSucursal, start, end, ct);
            var habitaciones = await _context.Habitaciones
                .AsNoTracking()
                .Where(h => h.IdSucursal == idSucursal && h.EstadoHabitacion == "DIS" && !ocupadas.Contains(h.IdHabitacion))
                .ToListAsync(ct);

            return new AccommodationAvailabilityDTO
            {
                FechaEntrada = start,
                FechaSalida = end,
                PorTipoHabitacion = tipos.Select(t => new AvailabilityByRoomTypeDTO
                {
                    TipoHabitacionGuid = t.TipoHabitacionGuid,
                    Nombre = t.NombreTipoHabitacion,
                    Disponibles = habitaciones.Count(h => h.IdTipoHabitacion == t.IdTipoHabitacion)
                }).ToList()
            };
        }

        private async Task<List<int>> GetHabitacionesOcupadasAsync(int idSucursal, DateTime fechaEntrada, DateTime fechaSalida, CancellationToken ct)
        {
            return await _context.ReservasHabitaciones
                .AsNoTracking()
                .Where(rh => rh.Reserva.IdSucursal == idSucursal
                    && ActiveReservationStates.Contains(rh.Reserva.EstadoReserva)
                    && rh.FechaInicio.Date < fechaSalida.Date
                    && rh.FechaFin.Date > fechaEntrada.Date)
                .Select(rh => rh.IdHabitacion)
                .Distinct()
                .ToListAsync(ct);
        }

        private async Task<Dictionary<int, List<string>>> GetImagenesPorTipoAsync(IEnumerable<int> tipoIds, CancellationToken ct)
        {
            var ids = tipoIds.Distinct().ToList();
            var images = await _context.TiposHabitacionImagenes
                .AsNoTracking()
                .Where(i => ids.Contains(i.IdTipoHabitacion))
                .OrderByDescending(i => i.EsPrincipal)
                .ThenBy(i => i.OrdenVisualizacion)
                .ToListAsync(ct);

            return images
                .GroupBy(i => i.IdTipoHabitacion)
                .ToDictionary(g => g.Key, g => g.Select(i => i.UrlImagen).Where(u => !string.IsNullOrWhiteSpace(u)).Distinct().ToList());
        }

        private async Task<string?> GetImagenPrincipalAsync(IEnumerable<int> tipoIds, CancellationToken ct)
        {
            var ids = tipoIds.Distinct().ToList();
            return await _context.TiposHabitacionImagenes
                .AsNoTracking()
                .Where(i => ids.Contains(i.IdTipoHabitacion))
                .OrderByDescending(i => i.EsPrincipal)
                .ThenBy(i => i.OrdenVisualizacion)
                .Select(i => i.UrlImagen)
                .FirstOrDefaultAsync(ct);
        }

        private async Task<List<string>> GetAmenitiesAsync(int idSucursal, IEnumerable<int> tipoIds, CancellationToken ct)
        {
            var ids = tipoIds.Distinct().ToList();
            var sucursalAmenities = _context.CatalogosServicios
                .AsNoTracking()
                .Where(c => c.IdSucursal == idSucursal && c.EstadoCatalogo == "ACT")
                .Select(c => c.NombreCatalogo);

            var tipoAmenities = _context.TiposHabitacionCatalogos
                .AsNoTracking()
                .Where(tc => ids.Contains(tc.IdTipoHabitacion) && tc.CatalogoServicio.EstadoCatalogo == "ACT")
                .Select(tc => tc.CatalogoServicio.NombreCatalogo);

            return await sucursalAmenities
                .Union(tipoAmenities)
                .Distinct()
                .OrderBy(a => a)
                .ToListAsync(ct);
        }

        private async Task<List<AccommodationTariffDTO>> GetTarifasActivasAsync(int idSucursal, List<TipoHabitacionEntity> tipos, CancellationToken ct)
        {
            var tipoMap = tipos.ToDictionary(t => t.IdTipoHabitacion, t => t.TipoHabitacionGuid);
            var now = DateTime.UtcNow.Date;
            var tarifas = await _context.Tarifas
                .AsNoTracking()
                .Where(t => t.IdSucursal == idSucursal
                    && tipoMap.Keys.Contains(t.IdTipoHabitacion)
                    && t.EstadoTarifa == "ACT"
                    && t.PermitePortalPublico
                    && t.FechaInicio.Date <= now
                    && t.FechaFin.Date >= now)
                .OrderBy(t => t.Prioridad)
                .ThenBy(t => t.PrecioPorNoche)
                .ToListAsync(ct);

            return tarifas.Select(t => new AccommodationTariffDTO
            {
                TarifaGuid = t.TarifaGuid,
                Nombre = t.NombreTarifa,
                PrecioPorNoche = t.PrecioPorNoche,
                FechaInicio = t.FechaInicio,
                FechaFin = t.FechaFin,
                MinNoches = t.MinNoches,
                TipoHabitacionGuid = tipoMap.TryGetValue(t.IdTipoHabitacion, out var guid) ? guid : null
            }).ToList();
        }

        private async Task<Dictionary<int, decimal>> GetPreciosBasePorTipoAsync(int idSucursal, CancellationToken ct)
        {
            return await _context.Habitaciones
                .AsNoTracking()
                .Where(h => h.IdSucursal == idSucursal && h.EstadoHabitacion == "DIS")
                .GroupBy(h => h.IdTipoHabitacion)
                .Select(g => new { IdTipoHabitacion = g.Key, Precio = g.Min(h => h.PrecioBase) })
                .ToDictionaryAsync(g => g.IdTipoHabitacion, g => g.Precio, ct);
        }

        private static BookingPagedResponseDTO<T> Page<T>(List<T> source, int pagina, int limite)
        {
            var total = source.Count;
            var items = source.Skip((pagina - 1) * limite).Take(limite).ToList();
            return BuildPaged(items, pagina, limite, total);
        }

        private static BookingPagedResponseDTO<T> BuildPaged<T>(List<T> items, int pagina, int limite, int total)
        {
            var totalPaginas = total == 0 ? 0 : (int)Math.Ceiling(total / (double)limite);
            return new BookingPagedResponseDTO<T>
            {
                Items = items,
                Pagina = pagina,
                Limite = limite,
                TotalResultados = total,
                TotalPaginas = totalPaginas,
                TieneAnterior = pagina > 1,
                TieneSiguiente = pagina < totalPaginas
            };
        }

        private static string BuildPolicyText(SucursalEntity sucursal)
        {
            var parts = new List<string>();
            if (sucursal.CheckinAnticipado) parts.Add("Check-in anticipado sujeto a disponibilidad.");
            if (sucursal.CheckoutTardio) parts.Add("Check-out tardio sujeto a disponibilidad.");
            if (sucursal.SePermiteFumar) parts.Add("Se permite fumar en areas autorizadas.");
            return parts.Count == 0 ? "Aplican politicas generales de la propiedad." : string.Join(" ", parts);
        }

        private static string? FirstNonEmpty(params string?[] values)
            => values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));

        private static bool Contains(string? source, string? value)
            => !string.IsNullOrWhiteSpace(source)
                && !string.IsNullOrWhiteSpace(value)
                && source.Contains(value, StringComparison.OrdinalIgnoreCase);
    }
}
