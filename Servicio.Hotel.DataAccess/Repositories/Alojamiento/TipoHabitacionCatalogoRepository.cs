using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento;

namespace Servicio.Hotel.DataAccess.Repositories.Alojamiento
{
    public class TipoHabitacionCatalogoRepository : RepositoryBase<TipoHabitacionCatalogoEntity>, ITipoHabitacionCatalogoRepository
    {
        public TipoHabitacionCatalogoRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<TipoHabitacionCatalogoEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<IEnumerable<TipoHabitacionCatalogoEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<TipoHabitacionCatalogoEntity> AddAsync(TipoHabitacionCatalogoEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task DeleteByTipoAndCatalogoAsync(int idTipoHabitacion, int idCatalogo, CancellationToken ct = default)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(thc => thc.IdTipoHabitacion == idTipoHabitacion && thc.IdCatalogo == idCatalogo, ct);
            if (entity != null)
                await DeleteAsync(entity.IdTipoHabCatalogo, ct);
        }

        public async Task<bool> ExistsAsync(int idTipoHabitacion, int idCatalogo, CancellationToken ct = default)
            => await _dbSet.AnyAsync(thc => thc.IdTipoHabitacion == idTipoHabitacion && thc.IdCatalogo == idCatalogo, ct);
    }
}