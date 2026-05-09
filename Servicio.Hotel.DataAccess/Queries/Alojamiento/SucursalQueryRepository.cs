using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataAccess.Common.Pagination;

namespace Servicio.Hotel.DataAccess.Queries.Alojamiento
{
    public class SucursalQuery
    {
        private readonly ServicioHotelDbContext _context;

        public SucursalQuery(ServicioHotelDbContext context)
        {
            _context = context;
        }

        // Búsqueda paginada de sucursales con filtros (complejo)
        public async Task<PagedResult<SucursalEntity>> SearchSucursalesAsync(
            string? destino,
            string? tipoAlojamiento,
            string? ciudad,
            int pagina,
            int limite,
            CancellationToken ct = default)
        {
            var query = _context.Sucursales
                .Where(s => !s.EsEliminado && s.EstadoSucursal == "ACT");

            if (!string.IsNullOrEmpty(destino))
                query = query.Where(s => s.NombreSucursal.Contains(destino) || s.Ciudad.Contains(destino));

            if (!string.IsNullOrEmpty(tipoAlojamiento))
                query = query.Where(s => s.TipoAlojamiento == tipoAlojamiento);

            if (!string.IsNullOrEmpty(ciudad))
                query = query.Where(s => s.Ciudad == ciudad);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip((pagina - 1) * limite)
                .Take(limite)
                .ToListAsync(ct);

            return new PagedResult<SucursalEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pagina,
                PageSize = limite
            };
        }

        // Obtener rating promedio de una sucursal (agregación compleja)
        public async Task<decimal?> GetRatingPromedioAsync(int idSucursal, CancellationToken ct = default)
        {
            return await _context.Valoraciones
                .Where(v => v.IdSucursal == idSucursal && v.EstadoValoracion == "PUB" && v.PublicadaEnPortal)
                .AverageAsync(v => (decimal?)v.PuntuacionGeneral, ct);
        }
    }
}