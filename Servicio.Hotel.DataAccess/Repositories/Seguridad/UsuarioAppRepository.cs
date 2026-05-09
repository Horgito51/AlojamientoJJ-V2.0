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
    public class UsuarioAppRepository : RepositoryBase<UsuarioAppEntity>, IUsuarioAppRepository
    {
        public UsuarioAppRepository(ServicioHotelDbContext context) : base(context) { }

        public override async Task<UsuarioAppEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _dbSet
                .Include(u => u.UsuariosRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == id, ct);

        public override async Task<IEnumerable<UsuarioAppEntity>> GetAllAsync(CancellationToken ct = default)
            => await _dbSet
                .Include(u => u.UsuariosRoles)
                    .ThenInclude(ur => ur.Rol)
                .ToListAsync(ct);

        public async Task<UsuarioAppEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet
                .Include(u => u.UsuariosRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.UsuarioGuid == guid, ct);

        public async Task<UsuarioAppEntity> AddAsync(UsuarioAppEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(UsuarioAppEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task<UsuarioAppEntity?> GetByUsernameAsync(string username, CancellationToken ct = default)
            => await _dbSet
                .Include(u => u.UsuariosRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Username == username, ct);

        public async Task<UsuarioAppEntity?> GetByCorreoAsync(string correo, CancellationToken ct = default)
            => await _dbSet
                .Include(u => u.UsuariosRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Correo == correo, ct);

        public async Task<UsuarioAppEntity?> AuthenticateAsync(string username, string passwordHash, CancellationToken ct = default)
            => await _dbSet
                .Include(u => u.UsuariosRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash && u.Activo && u.EstadoUsuario == "ACT", ct);

        public async Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null, CancellationToken ct = default)
        {
            var query = _dbSet.Where(u => u.Username == username);
            if (excludeId.HasValue) query = query.Where(u => u.IdUsuario != excludeId.Value);
            return await query.AnyAsync(ct);
        }

        public async Task<bool> ExistsByCorreoAsync(string correo, int? excludeId = null, CancellationToken ct = default)
        {
            var query = _dbSet.Where(u => u.Correo == correo);
            if (excludeId.HasValue) query = query.Where(u => u.IdUsuario != excludeId.Value);
            return await query.AnyAsync(ct);
        }

        public async Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default)
        {
            var user = await GetByIdAsync(id, ct);
            if (user != null)
            {
                user.EstadoUsuario = "BLO";
                user.Activo = false;
                user.FechaInhabilitacionUtc = DateTime.UtcNow;
                user.MotivoInhabilitacion = motivo;
                user.ModificadoPorUsuario = usuario;
                user.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(user, ct);
            }
        }

        public async Task CambiarPasswordAsync(int id, string newHash, string newSalt, string usuario, CancellationToken ct = default)
        {
            var user = await GetByIdAsync(id, ct);
            if (user != null)
            {
                user.PasswordHash = newHash;
                user.PasswordSalt = newSalt;
                user.ModificadoPorUsuario = usuario;
                user.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(user, ct);
            }
        }
    }
}