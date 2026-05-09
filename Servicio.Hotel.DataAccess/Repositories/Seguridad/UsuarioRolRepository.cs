using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Seguridad;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Seguridad;

namespace Servicio.Hotel.DataAccess.Repositories.Seguridad
{
    public class UsuarioRolRepository : RepositoryBase<UsuarioRolEntity>, IUsuarioRolRepository
    {
        public UsuarioRolRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<UsuarioRolEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<IEnumerable<UsuarioRolEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<UsuarioRolEntity> AddAsync(UsuarioRolEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        // No implementamos Update ni Delete genéricos porque la interfaz no los pide.

        public async Task DeleteByUsuarioAndRolAsync(int idUsuario, int idRol, CancellationToken ct = default)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(ur => ur.IdUsuario == idUsuario && ur.IdRol == idRol, ct);
            if (entity != null)
            {
                entity.EsEliminado = true;
                entity.Activo = false;
                entity.FechaModificacionUtc = System.DateTime.UtcNow;
                await UpdateAsync(entity, ct);
            }
        }

        public async Task DeleteAllByUsuarioAsync(int idUsuario, CancellationToken ct = default)
        {
            var entities = await _dbSet.Where(ur => ur.IdUsuario == idUsuario).ToListAsync(ct);
            foreach (var entity in entities)
            {
                entity.EsEliminado = true;
                entity.Activo = false;
                entity.FechaModificacionUtc = System.DateTime.UtcNow;
            }
            await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsAsync(int idUsuario, int idRol, CancellationToken ct = default)
            => await _dbSet.AnyAsync(ur => ur.IdUsuario == idUsuario && ur.IdRol == idRol && ur.Activo && !ur.EsEliminado, ct);

        public async Task<IEnumerable<int>> GetRolesIdsByUsuarioAsync(int idUsuario, CancellationToken ct = default)
            => await _dbSet.Where(ur => ur.IdUsuario == idUsuario && ur.Activo && !ur.EsEliminado)
                           .Select(ur => ur.IdRol)
                           .ToListAsync(ct);
    }
}