using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Facturacion;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Facturacion;

namespace Servicio.Hotel.DataAccess.Repositories.Facturacion
{
    public class FacturaRepository : RepositoryBase<FacturaEntity>, IFacturaRepository
    {
        private static void WriteDebugLog(string hypothesisId, string location, string message, string dataJson)
        {
            try
            {
                var payload = $"{{\"sessionId\":\"86bafb\",\"runId\":\"initial\",\"hypothesisId\":\"{hypothesisId}\",\"location\":\"{location}\",\"message\":\"{message}\",\"data\":{dataJson},\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}}}";
                File.AppendAllText(@"c:\Users\jorge\OneDrive\Escritorio\Puce\Semestre 6\Integracion de sistemas\Practicas\debug-86bafb.log", payload + Environment.NewLine);
            }
            catch
            {
                // ignore debug logger failures
            }
        }

        public FacturaRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<FacturaEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _dbSet
                .Include(f => f.FacturaDetalles)
                .FirstOrDefaultAsync(f => f.IdFactura == id, ct);

        public async Task<FacturaEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet
                .Include(f => f.FacturaDetalles)
                .FirstOrDefaultAsync(f => f.GuidFactura == guid, ct);

        public async Task<IEnumerable<FacturaEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<FacturaEntity> AddAsync(FacturaEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(FacturaEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task UpdateSaldoPendienteAsync(int idFactura, decimal nuevoSaldo, CancellationToken ct = default)
        {
            var factura = await GetByIdAsync(idFactura, ct);
            if (factura != null)
            {
                factura.SaldoPendiente = nuevoSaldo;
                if (nuevoSaldo == 0) factura.Estado = "PAG";
                await UpdateAsync(factura, ct);
            }
        }

        public async Task AnularAsync(int idFactura, string motivo, string usuario, CancellationToken ct = default)
        {
            var factura = await GetByIdAsync(idFactura, ct);
            if (factura != null)
            {
                factura.Estado = "ANU";
                factura.MotivoInhabilitacion = motivo;
                factura.ModificadoPorUsuario = usuario;
                factura.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(factura, ct);
            }
        }

        public async Task<bool> EstaPagadaAsync(int idFactura, CancellationToken ct = default)
        {
            var factura = await GetByIdAsync(idFactura, ct);
            return factura != null && factura.Estado == "PAG";
        }

        public async Task<int> GenerarFacturaReservaAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            var reserva = await _context.Reservas
                .Include(r => r.ReservasHabitaciones)
                .FirstOrDefaultAsync(r => r.IdReserva == idReserva, ct);

            if (reserva == null)
                throw new KeyNotFoundException($"No se encontró la reserva con ID {idReserva}.");

            // Crear cabecera de la factura
            var factura = new FacturaEntity
            {
                GuidFactura = Guid.NewGuid(),
                IdReserva = reserva.IdReserva,
                IdCliente = reserva.IdCliente,
                IdSucursal = reserva.IdSucursal,
                TipoFactura = "RESERVA",
                NumeroFactura = $"RES-{reserva.CodigoReserva}",
                FechaEmision = DateTime.UtcNow,
                Subtotal = reserva.SubtotalReserva,
                ValorIva = reserva.ValorIva,
                Total = reserva.TotalReserva,
                DescuentoTotal = reserva.DescuentoAplicado,
                SaldoPendiente = reserva.TotalReserva,
                Moneda = "USD",
                ObservacionesFactura = string.Empty,
                OrigenCanalFactura = "API_PUBLICA",
                Estado = "EMI",
                EsEliminado = false,
                CreadoPorUsuario = usuario,
                FechaRegistroUtc = DateTime.UtcNow,
                ModificadoPorUsuario = string.Empty,
                ModificacionIp = string.Empty,
                ServicioOrigen = "facturacion-service"
            };

            // #region agent log
            WriteDebugLog(
                "H6",
                "Servicio.Hotel.DataAccess/Repositories/Facturacion/FacturaRepository.cs:GenerarFacturaReservaAsync:beforeAdd",
                "Factura payload before insert",
                $"{{\"idReserva\":{idReserva},\"facturaMoneda\":{(factura.Moneda == null ? "null" : $"\\\"{factura.Moneda}\\\"")},\"reservaTotal\":{reserva.TotalReserva},\"reservaEstado\":\"{reserva.EstadoReserva}\"}}");
            // #endregion
            await _context.Facturas.AddAsync(factura, ct);
            try
            {
                await _context.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                // #region agent log
                WriteDebugLog(
                    "H7",
                    "Servicio.Hotel.DataAccess/Repositories/Facturacion/FacturaRepository.cs:GenerarFacturaReservaAsync:saveError",
                    "Factura insert failed",
                    $"{{\"idReserva\":{idReserva},\"message\":\"{(ex.Message ?? string.Empty).Replace("\"", "'")}\"}}");
                // #endregion
                throw;
            }

            // Crear detalles de la factura basados en las habitaciones
            foreach (var rh in reserva.ReservasHabitaciones)
            {
                var detalle = new FacturaDetalleEntity
                {
                    FacturaDetalleGuid = Guid.NewGuid(),
                    IdFactura = factura.IdFactura,
                    TipoItem = "ALOJAMIENTO",
                    ReferenciaId = rh.IdHabitacion,
                    DescripcionItem = $"Hospedaje Habitacion {rh.IdHabitacion} ({rh.FechaInicio:yyyy-MM-dd} al {rh.FechaFin:yyyy-MM-dd})",
                    Cantidad = (int)(rh.FechaFin.Date - rh.FechaInicio.Date).TotalDays,
                    PrecioUnitario = rh.PrecioNocheAplicado,
                    SubtotalLinea = rh.SubtotalLinea,
                    ValorIvaLinea = rh.ValorIvaLinea,
                    DescuentoLinea = rh.DescuentoLinea,
                    TotalLinea = rh.TotalLinea,
                    FechaRegistroUtc = DateTime.UtcNow,
                    CreadoPorUsuario = usuario
                };
                if (detalle.Cantidad <= 0) detalle.Cantidad = 1;

                await _context.FacturasDetalle.AddAsync(detalle, ct);
            }

            await _context.SaveChangesAsync(ct);
            return factura.IdFactura;
        }

        public async Task<int> GenerarFacturaFinalAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            var reserva = await _context.Reservas
                .Include(r => r.ReservasHabitaciones)
                .FirstOrDefaultAsync(r => r.IdReserva == idReserva, ct);

            if (reserva == null)
                throw new KeyNotFoundException($"No se encontró la reserva con ID {idReserva}.");

            var factura = new FacturaEntity
            {
                GuidFactura = Guid.NewGuid(),
                IdReserva = reserva.IdReserva,
                IdCliente = reserva.IdCliente,
                IdSucursal = reserva.IdSucursal,
                TipoFactura = "FINAL",
                NumeroFactura = $"FAC-{reserva.CodigoReserva}",
                FechaEmision = DateTime.UtcNow,
                Subtotal = reserva.SubtotalReserva,
                ValorIva = reserva.ValorIva,
                Total = reserva.TotalReserva,
                DescuentoTotal = reserva.DescuentoAplicado,
                SaldoPendiente = reserva.TotalReserva,
                Moneda = "USD",
                ObservacionesFactura = string.Empty,
                OrigenCanalFactura = "API_PUBLICA",
                Estado = "EMI",
                EsEliminado = false,
                CreadoPorUsuario = usuario,
                FechaRegistroUtc = DateTime.UtcNow,
                ModificadoPorUsuario = string.Empty,
                ModificacionIp = string.Empty,
                ServicioOrigen = "facturacion-service"
            };

            await _context.Facturas.AddAsync(factura, ct);
            await _context.SaveChangesAsync(ct);

            foreach (var rh in reserva.ReservasHabitaciones)
            {
                var detalle = new FacturaDetalleEntity
                {
                    FacturaDetalleGuid = Guid.NewGuid(),
                    IdFactura = factura.IdFactura,
                    TipoItem = "ALOJAMIENTO",
                    ReferenciaId = rh.IdHabitacion,
                    DescripcionItem = $"Liquidacion Hospedaje Habitacion {rh.IdHabitacion}",
                    Cantidad = (int)(rh.FechaFin.Date - rh.FechaInicio.Date).TotalDays,
                    PrecioUnitario = rh.PrecioNocheAplicado,
                    SubtotalLinea = rh.SubtotalLinea,
                    ValorIvaLinea = rh.ValorIvaLinea,
                    DescuentoLinea = rh.DescuentoLinea,
                    TotalLinea = rh.TotalLinea,
                    FechaRegistroUtc = DateTime.UtcNow,
                    CreadoPorUsuario = usuario
                };
                if (detalle.Cantidad <= 0) detalle.Cantidad = 1;

                await _context.FacturasDetalle.AddAsync(detalle, ct);
            }

            await _context.SaveChangesAsync(ct);
            return factura.IdFactura;
        }
    }
}
