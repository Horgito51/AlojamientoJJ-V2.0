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
    public class FacturaDetalleRepository : RepositoryBase<FacturaDetalleEntity>, IFacturaDetalleRepository
    {
        public FacturaDetalleRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<FacturaDetalleEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<FacturaDetalleEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(fd => fd.FacturaDetalleGuid == guid, ct);

        public async Task<IEnumerable<FacturaDetalleEntity>> GetByFacturaAsync(int idFactura, CancellationToken ct = default)
            => await _dbSet.Where(fd => fd.IdFactura == idFactura).ToListAsync(ct);

        public async Task<FacturaDetalleEntity> AddAsync(FacturaDetalleEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);
    }
}