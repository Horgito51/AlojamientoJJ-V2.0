using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento;

namespace Servicio.Hotel.DataAccess.Repositories.Alojamiento
{
    public class TipoHabitacionRepository : RepositoryBase<TipoHabitacionEntity>, ITipoHabitacionRepository
    {
        public TipoHabitacionRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<TipoHabitacionEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<TipoHabitacionEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(th => th.TipoHabitacionGuid == guid, ct);

        public async Task<TipoHabitacionEntity?> GetBySlugAsync(string slug, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(th => th.Slug == slug, ct);

        public async Task<IEnumerable<TipoHabitacionEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<TipoHabitacionEntity> AddAsync(TipoHabitacionEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(TipoHabitacionEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken ct = default)
            => await _dbSet.AnyAsync(th => th.CodigoTipoHabitacion == codigo, ct);

        public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken ct = default)
            => await _dbSet.AnyAsync(th => th.Slug == slug, ct);
        public async Task<IEnumerable<TipoHabitacionEntity>> GetPublicosAsync(CancellationToken ct = default)
        {
            return await _dbSet.Where(th => th.PermiteReservaPublica && th.EstadoTipoHabitacion == "ACT").ToListAsync(ct);
        }
    }
}
