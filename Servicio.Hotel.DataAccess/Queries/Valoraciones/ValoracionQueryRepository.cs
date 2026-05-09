using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Valoraciones;
using Servicio.Hotel.DataAccess.Common.Pagination;

namespace Servicio.Hotel.DataAccess.Queries.Valoraciones
{
    public class ValoracionQuery
    {
        private readonly ServicioHotelDbContext _context;

        public ValoracionQuery(ServicioHotelDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ValoracionEntity>> GetValoracionesPaginadasAsync(
            int? idSucursal,
            string? estado,
            string? tipoViaje,
            decimal? puntuacionMin,
            decimal? puntuacionMax,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            bool? publicada,
            int pagina,
            int limite,
            CancellationToken ct = default)
        {
            var query = _context.Valoraciones.AsQueryable();

            if (idSucursal.HasValue)
                query = query.Where(v => v.IdSucursal == idSucursal.Value);

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(v => v.EstadoValoracion == estado);

            if (!string.IsNullOrEmpty(tipoViaje))
                query = query.Where(v => v.TipoViaje == tipoViaje);

            if (puntuacionMin.HasValue)
                query = query.Where(v => v.PuntuacionGeneral >= puntuacionMin.Value);

            if (puntuacionMax.HasValue)
                query = query.Where(v => v.PuntuacionGeneral <= puntuacionMax.Value);

            if (fechaDesde.HasValue)
                query = query.Where(v => v.FechaRegistroUtc >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(v => v.FechaRegistroUtc <= fechaHasta.Value);

            if (publicada.HasValue)
                query = query.Where(v => v.PublicadaEnPortal == publicada.Value);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(v => v.FechaRegistroUtc)
                .Skip((pagina - 1) * limite)
                .Take(limite)
                .ToListAsync(ct);

            return new PagedResult<ValoracionEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pagina,
                PageSize = limite
            };
        }
    }
}