using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Seguridad;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Seguridad;

namespace Servicio.Hotel.DataAccess.Repositories.Seguridad
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        protected readonly ServicioHotelDbContext _context;
        protected readonly DbSet<AuditoriaEntity> _dbSet;

        public AuditoriaRepository(ServicioHotelDbContext context)
        {
            _context = context;
            _dbSet = context.Set<AuditoriaEntity>();
        }

        public async Task<AuditoriaEntity?> GetByIdAsync(long id, CancellationToken ct = default)
            => await _dbSet.FindAsync(new object[] { id }, ct);

        public async Task<IEnumerable<AuditoriaEntity>> GetAllAsync(CancellationToken ct = default)
            => await _dbSet.ToListAsync(ct);

        public async Task<AuditoriaEntity> AddAsync(AuditoriaEntity entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        // Nota: Update y Delete no están en la interfaz, pero si los necesitas, se pueden agregar.

        public async Task<AuditoriaEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(a => a.AuditoriaGuid == guid, ct);
    }
}