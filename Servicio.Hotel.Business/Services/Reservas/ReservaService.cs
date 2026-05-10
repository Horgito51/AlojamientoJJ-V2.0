using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Alojamiento;
using Servicio.Hotel.Business.Interfaces.Facturacion;
using Servicio.Hotel.Business.Interfaces.Reservas;
using Servicio.Hotel.Business.Mappers.Reservas;
using Servicio.Hotel.Business.Validators.Reservas;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataManagement.UnitOfWork;
using Servicio.Hotel.DataManagement.Reservas.Interfaces;

namespace Servicio.Hotel.Business.Services.Reservas
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaDataService _reservaDataService;
        private readonly ITarifaService _tarifaService;
        private readonly IHabitacionService _habitacionService;
        private readonly ITipoHabitacionService _tipoHabitacionService;
        private readonly IFacturaService _facturaService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ServicioHotelDbContext _context;

        public ReservaService(
            IReservaDataService reservaDataService,
            ITarifaService tarifaService,
            IHabitacionService habitacionService,
            ITipoHabitacionService tipoHabitacionService,
            IFacturaService facturaService,
            IUnitOfWork unitOfWork,
            ServicioHotelDbContext context)
        {
            _reservaDataService = reservaDataService;
            _tarifaService = tarifaService;
            _habitacionService = habitacionService;
            _tipoHabitacionService = tipoHabitacionService;
            _facturaService = facturaService;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<ReservaDTO> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var dataModel = await _reservaDataService.GetByIdAsync(id, ct);
            if (dataModel == null)
                throw new NotFoundException("RES-001", $"No se encontro la reserva con ID {id}.");
            return dataModel.ToDto();
        }

        public async Task<ReservaDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var dataModel = await _reservaDataService.GetByGuidAsync(guid, ct);
            if (dataModel == null)
                throw new NotFoundException("RES-002", $"No se encontro la reserva con GUID {guid}.");
            return dataModel.ToDto();
        }

        public async Task<ReservaDTO> GetByCodigoAsync(string codigo, CancellationToken ct = default)
        {
            var dataModel = await _reservaDataService.GetByCodigoAsync(codigo, ct);
            if (dataModel == null)
                throw new NotFoundException("RES-003", $"No se encontro la reserva con codigo {codigo}.");
            return dataModel.ToDto();
        }

        public async Task<PagedResult<ReservaDTO>> GetByFiltroAsync(ReservaFiltroDTO filtro, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var pagedData = await _reservaDataService.GetByFiltroAsync(filtro.ToDataModel(), pageNumber, pageSize, ct);
            return new PagedResult<ReservaDTO>
            {
                Items = pagedData.Items.ToDtoList(),
                TotalCount = pagedData.TotalCount,
                PageNumber = pagedData.PageNumber,
                PageSize = pagedData.PageSize
            };
        }

        public async Task<ReservaDTO> CreateAsync(ReservaCreateDTO reservaCreateDto, CancellationToken ct = default)
        {
            if (reservaCreateDto.Habitaciones == null || reservaCreateDto.Habitaciones.Count == 0)
                throw new ValidationException("RES-010", "Toda reserva debe tener al menos una habitacion.");

            decimal subtotalTotal = 0;
            const decimal ivaRate = 0.12m;

            foreach (var h in reservaCreateDto.Habitaciones)
            {
                var precio = await CalcularPrecioHabitacionAsync(
                    h.IdHabitacion,
                    h.FechaInicio == default ? reservaCreateDto.FechaInicio : h.FechaInicio,
                    h.FechaFin == default ? reservaCreateDto.FechaFin : h.FechaFin,
                    reservaCreateDto.OrigenCanalReserva,
                    ct);

                h.IdTarifa = precio.IdTarifa;
                h.PrecioNocheAplicado = precio.PrecioNocheAplicado;
                h.SubtotalLinea = precio.SubtotalLinea;
                h.ValorIvaLinea = precio.ValorIvaLinea;
                h.TotalLinea = precio.TotalLinea;

                subtotalTotal += h.SubtotalLinea;
            }

            reservaCreateDto.SubtotalReserva = subtotalTotal;
            reservaCreateDto.ValorIva = Math.Round(subtotalTotal * ivaRate, 2);
            reservaCreateDto.TotalReserva = subtotalTotal + reservaCreateDto.ValorIva - reservaCreateDto.DescuentoAplicado;
            reservaCreateDto.SaldoPendiente = reservaCreateDto.TotalReserva;

            var reservaDto = new ReservaDTO
            {
                IdCliente = reservaCreateDto.IdCliente,
                IdSucursal = reservaCreateDto.IdSucursal,
                FechaInicio = reservaCreateDto.FechaInicio,
                FechaFin = reservaCreateDto.FechaFin,
                SubtotalReserva = reservaCreateDto.SubtotalReserva,
                ValorIva = reservaCreateDto.ValorIva,
                TotalReserva = reservaCreateDto.TotalReserva,
                DescuentoAplicado = reservaCreateDto.DescuentoAplicado,
                SaldoPendiente = reservaCreateDto.SaldoPendiente,
                OrigenCanalReserva = reservaCreateDto.OrigenCanalReserva,
                EstadoReserva = reservaCreateDto.EstadoReserva,
                Observaciones = reservaCreateDto.Observaciones ?? string.Empty,
                EsWalkin = reservaCreateDto.EsWalkin,
                Habitaciones = reservaCreateDto.Habitaciones
            };

            ReservaValidator.Validate(reservaDto);
            await EnsureHabitacionesDisponiblesAsync(reservaDto, null, ct);
            var dataModel = reservaDto.ToDataModel();
            var created = await _reservaDataService.AddAsync(dataModel, ct);
            return created.ToDto();
        }

        public async Task<ReservaDTO> CreateByTipoHabitacionAsync(ReservaPorTipoHabitacionCreateDTO reservaCreateDto, CancellationToken ct = default)
        {
            ValidateReservaPorTipoHabitacion(reservaCreateDto);

            ReservaDTO? created = null;
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var detalles = new List<ReservaHabitacionDTO>();

                foreach (var request in reservaCreateDto.Habitaciones)
                {
                    var tipo = await _tipoHabitacionService.GetByGuidAsync(request.TipoHabitacionGuid, ct);
                    if (tipo.IdTipoHabitacion <= 0)
                        throw new ValidationException("RES-TIPO-005", "tipoHabitacionGuid no es valido.");

                    if (!string.Equals(tipo.EstadoTipoHabitacion, "ACT", StringComparison.OrdinalIgnoreCase) ||
                        (reservaCreateDto.ExigirPermiteReservaPublica && !tipo.PermiteReservaPublica))
                        throw new ValidationException("RES-TIPO-006", $"El tipo de habitacion {request.TipoHabitacionGuid} no permite reserva publica.");

                    if (request.NumAdultos > tipo.CapacidadAdultos || request.NumNinos > tipo.CapacidadNinos)
                        throw new ValidationException("RES-TIPO-007", $"La capacidad solicitada excede el tipo de habitacion {tipo.NombreTipoHabitacion}.");

                    var habitacionesAsignadas = await GetHabitacionesDisponiblesForUpdateAsync(
                        reservaCreateDto.IdSucursal,
                        tipo.IdTipoHabitacion,
                        request.NumHabitaciones,
                        reservaCreateDto.FechaInicio,
                        reservaCreateDto.FechaFin,
                        ct);

                    if (habitacionesAsignadas.Count < request.NumHabitaciones)
                    {
                        throw new ConflictException(
                            $"No hay suficientes habitaciones disponibles para el tipo {tipo.NombreTipoHabitacion}. Solicitadas: {request.NumHabitaciones}, disponibles: {habitacionesAsignadas.Count}.");
                    }

                    detalles.AddRange(habitacionesAsignadas.Select(h => new ReservaHabitacionDTO
                    {
                        IdHabitacion = h.IdHabitacion,
                        FechaInicio = reservaCreateDto.FechaInicio,
                        FechaFin = reservaCreateDto.FechaFin,
                        NumAdultos = request.NumAdultos,
                        NumNinos = request.NumNinos,
                        EstadoDetalle = "PEN"
                    }));
                }

                var createDto = new ReservaCreateDTO
                {
                    IdCliente = reservaCreateDto.IdCliente,
                    IdSucursal = reservaCreateDto.IdSucursal,
                    FechaInicio = reservaCreateDto.FechaInicio,
                    FechaFin = reservaCreateDto.FechaFin,
                    DescuentoAplicado = reservaCreateDto.DescuentoAplicado,
                    OrigenCanalReserva = string.IsNullOrWhiteSpace(reservaCreateDto.OrigenCanalReserva)
                        ? "MARKETPLACE"
                        : reservaCreateDto.OrigenCanalReserva,
                    EstadoReserva = "PEN",
                    Observaciones = reservaCreateDto.Observaciones ?? string.Empty,
                    EsWalkin = reservaCreateDto.EsWalkin,
                    Habitaciones = detalles
                };

                created = await CreateAsync(createDto, ct);
            }, ct);

            return created!;
        }

        public async Task<ReservaPrecioDTO> CalcularPrecioHabitacionAsync(int idHabitacion, DateTime fechaInicio, DateTime fechaFin, string? canal = null, CancellationToken ct = default)
        {
            if (fechaFin <= fechaInicio)
                throw new ValidationException("RES-PRECIO-001", "La fecha de fin debe ser posterior a la fecha de inicio.");

            var habitacion = await _habitacionService.GetByIdAsync(idHabitacion, ct);
            EnsureHabitacionReservable(habitacion);
            var tarifa = await _tarifaService.GetTarifaVigenteRangoOrDefaultAsync(
                habitacion.IdSucursal,
                habitacion.IdTipoHabitacion,
                fechaInicio,
                fechaFin,
                canal,
                ct);

            var noches = (int)(fechaFin.Date - fechaInicio.Date).TotalDays;
            if (noches <= 0) noches = 1;

            var precioNoche = tarifa?.PrecioPorNoche ?? habitacion.PrecioBase;
            var subtotal = Math.Round(precioNoche * noches, 2);
            var iva = Math.Round(subtotal * 0.12m, 2);

            return new ReservaPrecioDTO
            {
                IdHabitacion = habitacion.IdHabitacion,
                HabitacionGuid = habitacion.HabitacionGuid,
                IdSucursal = habitacion.IdSucursal,
                IdTarifa = tarifa?.IdTarifa,
                PrecioNocheAplicado = precioNoche,
                SubtotalLinea = subtotal,
                ValorIvaLinea = iva,
                TotalLinea = subtotal + iva,
                OrigenPrecio = tarifa == null ? "PRECIO_BASE" : "TARIFA"
            };
        }

        public async Task UpdateAsync(ReservaUpdateDTO reservaUpdateDto, CancellationToken ct = default)
        {
            var reservaDto = new ReservaDTO
            {
                IdReserva = reservaUpdateDto.IdReserva,
                FechaInicio = reservaUpdateDto.FechaInicio,
                FechaFin = reservaUpdateDto.FechaFin,
                SubtotalReserva = reservaUpdateDto.SubtotalReserva,
                ValorIva = reservaUpdateDto.ValorIva,
                TotalReserva = reservaUpdateDto.TotalReserva,
                DescuentoAplicado = reservaUpdateDto.DescuentoAplicado,
                SaldoPendiente = reservaUpdateDto.SaldoPendiente,
                EstadoReserva = reservaUpdateDto.EstadoReserva,
                Observaciones = reservaUpdateDto.Observaciones ?? string.Empty
            };

            var existing = await _reservaDataService.GetByIdAsync(reservaUpdateDto.IdReserva, ct);
            if (existing == null)
                throw new NotFoundException("RES-004", $"No se encontro la reserva con ID {reservaUpdateDto.IdReserva}.");

            if (existing.EstadoReserva is "CAN" or "FIN")
                throw new ConflictException("No se puede modificar una reserva cancelada o finalizada.");

            reservaDto.IdCliente = existing.IdCliente;
            reservaDto.IdSucursal = existing.IdSucursal;
            reservaDto.OrigenCanalReserva = existing.OrigenCanalReserva;
            reservaDto.Habitaciones = (existing.Habitaciones?.ToDtoList() ?? new List<ReservaHabitacionDTO>())
                .Select(h =>
                {
                    h.FechaInicio = reservaUpdateDto.FechaInicio;
                    h.FechaFin = reservaUpdateDto.FechaFin;
                    return h;
                })
                .ToList();

            ReservaValidator.Validate(reservaDto);
            await EnsureHabitacionesDisponiblesAsync(reservaDto, reservaUpdateDto.IdReserva, ct);

            existing.FechaInicio = reservaDto.FechaInicio;
            existing.FechaFin = reservaDto.FechaFin;
            existing.SubtotalReserva = reservaDto.SubtotalReserva;
            existing.ValorIva = reservaDto.ValorIva;
            existing.TotalReserva = reservaDto.TotalReserva;
            existing.DescuentoAplicado = reservaDto.DescuentoAplicado;
            existing.SaldoPendiente = reservaDto.SaldoPendiente;
            existing.EstadoReserva = reservaDto.EstadoReserva;
            existing.Observaciones = reservaDto.Observaciones;

            await _reservaDataService.UpdateAsync(existing, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _reservaDataService.GetByIdAsync(id, ct);
            if (existing == null)
                throw new NotFoundException("RES-005", $"No se encontro la habitacion con ID {id}.");
            await _reservaDataService.DeleteAsync(id, ct);
        }

        public async Task ConfirmarAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            var existing = await _reservaDataService.GetByIdAsync(idReserva, ct);
            if (existing == null)
                throw new NotFoundException("RES-006", $"No se encontro la reserva con ID {idReserva}.");

            if (existing.EstadoReserva == "CON")
                throw new ConflictException("La reserva ya esta confirmada.");

            if (existing.EstadoReserva != "PEN")
                throw new ConflictException($"No se puede confirmar una reserva en estado '{existing.EstadoReserva}'.");

            if (existing.Habitaciones == null || existing.Habitaciones.Count == 0)
                throw new ValidationException("RES-010", "La reserva no tiene habitaciones asociadas.");

            var habitacionesConConflicto = new HashSet<int>();
            foreach (var detalle in existing.Habitaciones)
            {
                var habitacion = await _habitacionService.GetByIdAsync(detalle.IdHabitacion, ct);
                EnsureHabitacionReservable(habitacion);

                var solapa = await _reservaDataService.ExisteSolapamientoAsync(
                    detalle.IdHabitacion,
                    detalle.FechaInicio,
                    detalle.FechaFin,
                    excludeIdReserva: idReserva,
                    ct: ct);

                if (solapa)
                    habitacionesConConflicto.Add(detalle.IdHabitacion);
            }

            if (habitacionesConConflicto.Count > 0)
            {
                var ids = string.Join(", ", habitacionesConConflicto);
                throw new ConflictException($"No se puede confirmar la reserva: las habitaciones {ids} ya estan ocupadas en el rango {existing.FechaInicio:yyyy-MM-dd} a {existing.FechaFin:yyyy-MM-dd}.");
            }

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await _reservaDataService.ConfirmarAsync(idReserva, usuario, ct);
                await _facturaService.GenerarFacturaReservaAsync(idReserva, usuario, ct);
            }, ct);
        }

        public async Task CancelarAsync(int idReserva, string motivo, string usuario, CancellationToken ct = default)
        {
            var existing = await _reservaDataService.GetByIdAsync(idReserva, ct);
            if (existing == null)
                throw new NotFoundException("RES-007", $"No se encontro la reserva con ID {idReserva}.");

            if (existing.EstadoReserva == "CAN")
                throw new ConflictException("La reserva ya esta cancelada.");

            if (existing.EstadoReserva != "PEN" && existing.EstadoReserva != "CON")
                throw new ConflictException($"No se puede cancelar una reserva en estado '{existing.EstadoReserva}'.");

            await _reservaDataService.CancelarAsync(idReserva, motivo, usuario, ct);
        }

        public async Task FinalizarAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            var existing = await _reservaDataService.GetByIdAsync(idReserva, ct);
            if (existing == null)
                throw new NotFoundException("RES-008", $"No se encontro la reserva con ID {idReserva}.");
            await _reservaDataService.FinalizarAsync(idReserva, usuario, ct);
        }

        public async Task<bool> PuedeCancelarAsync(int idReserva, CancellationToken ct = default)
        {
            return await _reservaDataService.PuedeCancelarAsync(idReserva, ct);
        }

        public async Task<int> ConfirmarReservaHabitacionAsync(int idReserva, int idHabitacion, int? idTarifa, DateTime fechaInicio, DateTime fechaFin, int numAdultos, int numNinos, decimal precioNoche, string usuario, CancellationToken ct = default)
        {
            if (fechaFin <= fechaInicio)
                throw new ValidationException("RES-009", "La fecha de fin debe ser posterior a la fecha de inicio.");
            return await _reservaDataService.ConfirmarReservaHabitacionAsync(idReserva, idHabitacion, idTarifa, fechaInicio, fechaFin, numAdultos, numNinos, precioNoche, usuario, ct);
        }

        private static void EnsureHabitacionReservable(Servicio.Hotel.Business.DTOs.Alojamiento.HabitacionDTO habitacion)
        {
            var estado = (habitacion.EstadoHabitacion ?? string.Empty).Trim().ToUpperInvariant();
            if (estado is "MNT" or "FDS" or "OCU" or "INA")
                throw new ConflictException($"La habitacion {habitacion.IdHabitacion} no esta disponible para reservar. Estado actual: {estado}.");
        }

        private static void ValidateReservaPorTipoHabitacion(ReservaPorTipoHabitacionCreateDTO reserva)
        {
            if (reserva.IdCliente <= 0)
                throw new ValidationException("RES-TIPO-001", "IdCliente es obligatorio.");
            if (reserva.IdSucursal <= 0)
                throw new ValidationException("RES-TIPO-002", "IdSucursal es obligatorio.");
            if (reserva.FechaFin <= reserva.FechaInicio)
                throw new ValidationException("RES-TIPO-003", "La fecha de fin debe ser posterior a la fecha de inicio.");
            if (reserva.Habitaciones == null || reserva.Habitaciones.Count == 0)
                throw new ValidationException("RES-TIPO-004", "Toda reserva debe solicitar al menos un tipo de habitacion.");

            foreach (var item in reserva.Habitaciones)
            {
                if (item.TipoHabitacionGuid == Guid.Empty)
                    throw new ValidationException("RES-TIPO-008", "tipoHabitacionGuid es obligatorio.");
                if (item.NumHabitaciones <= 0)
                    throw new ValidationException("RES-TIPO-009", "numHabitaciones debe ser mayor a cero.");
                if (item.NumAdultos <= 0)
                    throw new ValidationException("RES-TIPO-010", "numAdultos debe ser mayor a cero.");
                if (item.NumNinos < 0)
                    throw new ValidationException("RES-TIPO-011", "numNinos no puede ser negativo.");
            }
        }

        private async Task<List<HabitacionEntity>> GetHabitacionesDisponiblesForUpdateAsync(
            int idSucursal,
            int idTipoHabitacion,
            int cantidad,
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken ct)
        {
            return await _context.Habitaciones
                .FromSqlInterpolated($@"
                    SELECT TOP({cantidad}) h.*
                    FROM booking.HABITACION AS h WITH (UPDLOCK, HOLDLOCK, ROWLOCK)
                    WHERE h.id_sucursal = {idSucursal}
                        AND h.id_tipo_habitacion = {idTipoHabitacion}
                        AND h.estado_habitacion = 'DIS'
                        AND h.es_eliminado = 0
                        AND NOT EXISTS (
                            SELECT 1
                            FROM booking.RESERVAS_HABITACIONES AS rh WITH (UPDLOCK, HOLDLOCK)
                            INNER JOIN booking.RESERVAS AS r WITH (UPDLOCK, HOLDLOCK)
                                ON r.id_reserva = rh.id_reserva
                            WHERE rh.id_habitacion = h.id_habitacion
                                AND r.estado_reserva IN ('PEN', 'CON')
                                AND rh.estado_detalle IN ('PEN', 'CON')
                                AND rh.fecha_inicio < {fechaFin}
                                AND rh.fecha_fin > {fechaInicio}
                        )
                    ORDER BY h.id_habitacion")
                .AsTracking()
                .ToListAsync(ct);
        }

        private async Task EnsureHabitacionesDisponiblesAsync(ReservaDTO reserva, int? excludeIdReserva, CancellationToken ct)
        {
            if (reserva.Habitaciones == null || reserva.Habitaciones.Count == 0)
                throw new ValidationException("RES-010", "Toda reserva debe tener al menos una habitacion.");

            foreach (var detalle in reserva.Habitaciones)
            {
                if (detalle.IdHabitacion <= 0)
                    throw new ValidationException("RES-011", "Cada habitacion de la reserva debe tener un identificador valido.");

                var fechaInicio = detalle.FechaInicio == default ? reserva.FechaInicio : detalle.FechaInicio;
                var fechaFin = detalle.FechaFin == default ? reserva.FechaFin : detalle.FechaFin;

                if (fechaFin <= fechaInicio)
                    throw new ValidationException("RES-009", "La fecha de fin debe ser posterior a la fecha de inicio.");

                var habitacion = await _habitacionService.GetByIdAsync(detalle.IdHabitacion, ct);
                EnsureHabitacionReservable(habitacion);

                if (habitacion.IdSucursal != reserva.IdSucursal)
                    throw new ValidationException("RES-012", $"La habitacion {detalle.IdHabitacion} no pertenece a la sucursal de la reserva.");

                var solapa = await _reservaDataService.ExisteSolapamientoAsync(
                    detalle.IdHabitacion,
                    fechaInicio,
                    fechaFin,
                    excludeIdReserva,
                    ct);

                if (solapa)
                    throw new ConflictException($"La habitacion {detalle.IdHabitacion} ya tiene una reserva activa en esas fechas.");
            }
        }
    }
}
