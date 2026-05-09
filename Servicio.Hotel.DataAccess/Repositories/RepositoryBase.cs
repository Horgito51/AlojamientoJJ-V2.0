using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;

namespace Servicio.Hotel.DataAccess.Repositories
{
    public abstract class RepositoryBase<T> where T : class
    {
        protected readonly ServicioHotelDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected RepositoryBase(ServicioHotelDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _dbSet.FindAsync(new object[] { id }, ct);
            if (entity != null)
            {
                // Evita conflictos de tracking cuando se hace "Get + Update" con instancias distintas
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbSet.ToListAsync(ct);
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(ct);
        }

        public virtual async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await GetByIdAsync(id, ct);
            if (entity != null)
            {
                var entityType = entity.GetType();
                var esEliminadoProp = entityType.GetProperty("EsEliminado", BindingFlags.Public | BindingFlags.Instance);

                if (esEliminadoProp != null && esEliminadoProp.PropertyType == typeof(bool) && esEliminadoProp.CanWrite)
                {
                    esEliminadoProp.SetValue(entity, true);

                    var fechaModProp = entityType.GetProperty("FechaModificacionUtc", BindingFlags.Public | BindingFlags.Instance);
                    if (fechaModProp != null && fechaModProp.PropertyType == typeof(DateTime?) && fechaModProp.CanWrite)
                        fechaModProp.SetValue(entity, DateTime.UtcNow);

                    var modificadoPorProp = entityType.GetProperty("ModificadoPorUsuario", BindingFlags.Public | BindingFlags.Instance);
                    if (modificadoPorProp != null && modificadoPorProp.PropertyType == typeof(string) && modificadoPorProp.CanWrite)
                    {
                        var current = (string?)modificadoPorProp.GetValue(entity);
                        if (string.IsNullOrWhiteSpace(current))
                            modificadoPorProp.SetValue(entity, "Sistema");
                    }

                    _dbSet.Update(entity);
                    await _context.SaveChangesAsync(ct);
                    return;
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync(ct);
            }
        }
    }
}
