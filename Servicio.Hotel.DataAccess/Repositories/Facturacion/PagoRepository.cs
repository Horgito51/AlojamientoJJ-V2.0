using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Facturacion;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Facturacion;

namespace Servicio.Hotel.DataAccess.Repositories.Facturacion
{
    public class PagoRepository : RepositoryBase<PagoEntity>, IPagoRepository
    {
        public PagoRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<PagoEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<PagoEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(p => p.PagoGuid == guid, ct);

        public async Task<IEnumerable<PagoEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<PagoEntity> AddAsync(PagoEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(PagoEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task UpdateEstadoAsync(int idPago, string nuevoEstado, string usuario, CancellationToken ct = default)
        {
            var pago = await GetByIdAsync(idPago, ct);
            if (pago != null)
            {
                pago.EstadoPago = nuevoEstado;
                pago.ModificadoPorUsuario = usuario;
                pago.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(pago, ct);
            }
        }

        public async Task<bool> ExistsTransaccionExternaAsync(string transaccion, CancellationToken ct = default)
            => await _dbSet.AnyAsync(p => p.TransaccionExterna == transaccion, ct);
        public async Task<IEnumerable<PagoEntity>> GetByFacturaAsync(int idFactura, CancellationToken ct = default)
        {
            return await _dbSet.Where(p => p.IdFactura == idFactura).ToListAsync(ct);
        }

        public async Task<decimal> GetTotalPagadoPorFacturaAsync(int idFactura, CancellationToken ct = default)
        {
            return await _dbSet.Where(p => p.IdFactura == idFactura && p.EstadoPago == "APR")
                               .SumAsync(p => p.Monto, ct);
        }
    }

}