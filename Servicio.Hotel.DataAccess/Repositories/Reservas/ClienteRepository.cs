using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataAccess.Entities.Reservas;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Reservas;

namespace Servicio.Hotel.DataAccess.Repositories.Reservas
{
    public class ClienteRepository : RepositoryBase<ClienteEntity>, IClienteRepository
    {
        public ClienteRepository(ServicioHotelDbContext context) : base(context) { }

        public async Task<ClienteEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await base.GetByIdAsync(id, ct);

        public async Task<IEnumerable<ClienteEntity>> GetAllAsync(CancellationToken ct = default)
            => await base.GetAllAsync(ct);

        public async Task<ClienteEntity> AddAsync(ClienteEntity entity, CancellationToken ct = default)
            => await base.AddAsync(entity, ct);

        public async Task UpdateAsync(ClienteEntity entity, CancellationToken ct = default)
            => await base.UpdateAsync(entity, ct);

        public async Task DeleteAsync(int id, CancellationToken ct = default)
            => await base.DeleteAsync(id, ct);

        public async Task<ClienteEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(c => c.ClienteGuid == guid, ct);

        public async Task<ClienteEntity?> GetByIdentificacionAsync(string tipo, string numero, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(c => c.TipoIdentificacion == tipo && c.NumeroIdentificacion == numero, ct);

        public async Task<ClienteEntity?> GetByCorreoAsync(string correo, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(c => c.Correo == correo, ct);

        public async Task<bool> ExistsByIdentificacionAsync(string tipo, string numero, CancellationToken ct = default)
            => await _dbSet.AnyAsync(c => c.TipoIdentificacion == tipo && c.NumeroIdentificacion == numero, ct);

        public async Task<bool> ExistsByCorreoAsync(string correo, int? excludeId = null, CancellationToken ct = default)
        {
            var query = _dbSet.Where(c => c.Correo == correo);
            if (excludeId.HasValue) query = query.Where(c => c.IdCliente != excludeId.Value);
            return await query.AnyAsync(ct);
        }

        public async Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default)
        {
            var cliente = await GetByIdAsync(id, ct);
            if (cliente != null)
            {
                cliente.Estado = "INA";
                cliente.FechaInhabilitacionUtc = DateTime.UtcNow;
                cliente.MotivoInhabilitacion = motivo;
                cliente.ModificadoPorUsuario = usuario;
                cliente.FechaModificacionUtc = DateTime.UtcNow;
                await UpdateAsync(cliente, ct);
            }
        }
    }
}