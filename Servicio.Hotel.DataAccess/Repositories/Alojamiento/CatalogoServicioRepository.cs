using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento;

namespace Servicio.Hotel.DataAccess.Repositories.Alojamiento
{
    public class CatalogoServicioRepository : RepositoryBase<CatalogoServicioEntity>, ICatalogoServicioRepository
    {
        public CatalogoServicioRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<CatalogoServicioEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<CatalogoServicioEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(c => c.CatalogoGuid == guid, ct);

        public async Task<IEnumerable<CatalogoServicioEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<CatalogoServicioEntity> AddAsync(CatalogoServicioEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(CatalogoServicioEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken ct = default)
            => await _dbSet.AnyAsync(c => c.CodigoCatalogo == codigo, ct);
    }
}