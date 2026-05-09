using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Reservas;
using Servicio.Hotel.DataAccess.Common.Pagination;

namespace Servicio.Hotel.DataAccess.Queries.Reservas
{
    public class ReservaQuery
    {
        private readonly ServicioHotelDbContext _context;

        public ReservaQuery(ServicioHotelDbContext context)
        {
            _context = context;
        }

        // Listado paginado de reservas con filtros complejos
        public async Task<PagedResult<ReservaEntity>> GetReservasPaginadasAsync(
            string? estado,
            int? idSucursal,
            int? idCliente,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int pagina,
            int limite,
            CancellationToken ct = default)
        {
            var query = _context.Reservas
                .Include(r => r.ReservasHabitaciones)
                .Where(r => !r.EsEliminado);

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(r => r.EstadoReserva == estado);

            if (idSucursal.HasValue)
                query = query.Where(r => r.IdSucursal == idSucursal.Value);

            if (idCliente.HasValue)
                query = query.Where(r => r.IdCliente == idCliente.Value);

            if (fechaInicio.HasValue)
                query = query.Where(r => r.FechaInicio >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(r => r.FechaFin <= fechaFin.Value);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(r => r.FechaReservaUtc)
                .Skip((pagina - 1) * limite)
                .Take(limite)
                .ToListAsync(ct);

            return new PagedResult<ReservaEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pagina,
                PageSize = limite
            };
        }

        // Obtener reserva con todas sus habitaciones (carga ansiosa)
        public async Task<ReservaEntity?> GetReservaWithHabitacionesAsync(Guid reservaGuid, CancellationToken ct = default)
        {
            return await _context.Reservas
                .Include(r => r.ReservasHabitaciones)
                .FirstOrDefaultAsync(r => r.GuidReserva == reservaGuid && !r.EsEliminado, ct);
        }
    }
}