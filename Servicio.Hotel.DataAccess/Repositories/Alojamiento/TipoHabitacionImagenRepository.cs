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
    public class TipoHabitacionImagenRepository : RepositoryBase<TipoHabitacionImagenEntity>, ITipoHabitacionImagenRepository
    {
        public TipoHabitacionImagenRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<TipoHabitacionImagenEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<IEnumerable<TipoHabitacionImagenEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<TipoHabitacionImagenEntity> AddAsync(TipoHabitacionImagenEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(TipoHabitacionImagenEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task SetPrincipalAsync(int idTipoHabitacion, int idImagen, CancellationToken ct = default)
        {
            var imagenes = await _dbSet.Where(i => i.IdTipoHabitacion == idTipoHabitacion).ToListAsync(ct);
            foreach (var img in imagenes)
                img.EsPrincipal = (img.IdTipoHabitacionImagen == idImagen);
            await _context.SaveChangesAsync(ct);
        }

        public async Task ReordenarAsync(int idTipoHabitacion, Dictionary<int, int> ordenes, CancellationToken ct = default)
        {
            var imagenes = await _dbSet.Where(i => i.IdTipoHabitacion == idTipoHabitacion).ToListAsync(ct);
            foreach (var img in imagenes)
            {
                if (ordenes.TryGetValue(img.IdTipoHabitacionImagen, out int nuevoOrden))
                    img.OrdenVisualizacion = nuevoOrden;
            }
            await _context.SaveChangesAsync(ct);
        }
    }
}