using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Facturacion;
using Servicio.Hotel.DataAccess.Common.Pagination;

namespace Servicio.Hotel.DataAccess.Queries.Facturacion
{
    public class FacturaQuery
    {
        private readonly ServicioHotelDbContext _context;

        public FacturaQuery(ServicioHotelDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<FacturaEntity>> GetFacturasPaginadasAsync(
            string? estado,
            string? tipoFactura,
            int? idCliente,
            int? idReserva,
            DateTime? fechaDesde,
            DateTime? fechaHasta,
            int pagina,
            int limite,
            CancellationToken ct = default)
        {
            var query = _context.Facturas
                .Include(f => f.FacturaDetalles)
                .Where(f => !f.EsEliminado);

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(f => f.Estado == estado);

            if (!string.IsNullOrEmpty(tipoFactura))
                query = query.Where(f => f.TipoFactura == tipoFactura);

            if (idCliente.HasValue)
                query = query.Where(f => f.IdCliente == idCliente.Value);

            if (idReserva.HasValue)
                query = query.Where(f => f.IdReserva == idReserva.Value);

            if (fechaDesde.HasValue)
                query = query.Where(f => f.FechaEmision >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(f => f.FechaEmision <= fechaHasta.Value);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(f => f.FechaEmision)
                .Skip((pagina - 1) * limite)
                .Take(limite)
                .ToListAsync(ct);

            return new PagedResult<FacturaEntity>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pagina,
                PageSize = limite
            };
        }

        public async Task<FacturaEntity?> GetFacturaWithDetalleAsync(Guid facturaGuid, CancellationToken ct = default)
        {
            return await _context.Facturas
                .Include(f => f.FacturaDetalles)
                .FirstOrDefaultAsync(f => f.GuidFactura == facturaGuid && !f.EsEliminado, ct);
        }
    }
}