using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Seguridad;
using Servicio.Hotel.DataAccess.Common.Pagination;

namespace Servicio.Hotel.DataAccess.Queries.Seguridad
{
    public class AuditoriaQuery
    {
        private readonly ServicioHotelDbContext _context;

        public AuditoriaQuery(ServicioHotelDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<AuditoriaEntity>> GetAuditoriaPaginadaAsync(
            string? tabla,
            string? operacion,
            string? usuario,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int pagina,
            int limite,
            CancellationToken ct = default)
        {
            var query = _context.Auditorias.AsQueryable();

            if (!string.IsNullOrEmpty(tabla))
                query = query.Where(a => a.TablaAfectada == tabla);

            if (!string.IsNullOrEmpty(operacion))
                query = query.Where(a => a.Operacion == operacion);

            if (!string.IsNullOrEmpty(usuario))
                query = query.Where(a => a.UsuarioEjecutor == usuario);

            if (fechaDesde.HasValue)
                query = query.Where(a => a.FechaEventoUtc >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(a => a.FechaEventoUtc <= fechaHasta.Value);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(a => a.FechaEventoUtc)
                .Skip((pagina - 1) * limite)
                .Take(limite)
                .ToListAsync(ct);

            return new PagedResult<AuditoriaEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pagina,
                PageSize = limite
            };
        }

        public async Task<IEnumerable<AuditoriaEntity>> GetChangesByRegistroAsync(string tabla, string idRegistro, CancellationToken ct = default)
        {
            return await _context.Auditorias
                .Where(a => a.TablaAfectada == tabla && a.IdRegistroAfectado == idRegistro)
                .OrderByDescending(a => a.FechaEventoUtc)
                .ToListAsync(ct);
        }
    }
}