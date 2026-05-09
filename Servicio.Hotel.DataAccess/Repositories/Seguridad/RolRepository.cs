using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Seguridad;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Seguridad;

namespace Servicio.Hotel.DataAccess.Repositories.Seguridad
{
    public class RolRepository : RepositoryBase<RolEntity>, IRolRepository
    {
        public RolRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<RolEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<IEnumerable<RolEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<RolEntity> AddAsync(RolEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(RolEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task<RolEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(r => r.RolGuid == guid, ct);

        public async Task<RolEntity?> GetByNombreAsync(string nombre, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(r => r.NombreRol == nombre, ct);

        public async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null, CancellationToken ct = default)
        {
            var query = _dbSet.Where(r => r.NombreRol == nombre);
            if (excludeId.HasValue) query = query.Where(r => r.IdRol != excludeId.Value);
            return await query.AnyAsync(ct);
        }

        public async Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default)
        {
            var rol = await GetByIdAsync(id, ct);
            if (rol != null)
            {
                rol.EstadoRol = "INA";
                rol.Activo = false;
                rol.FechaInhabilitacionUtc = DateTime.UtcNow;
                rol.MotivoInhabilitacion = motivo;
                rol.ModificadoPorUsuario = usuario;
                rol.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(rol, ct);
            }
        }
    }
}