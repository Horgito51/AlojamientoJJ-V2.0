using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Reservas;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Reservas
{
    public interface IClienteRepository
    {
        // CRUD básico
        Task<ClienteEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ClienteEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<ClienteEntity>> GetAllAsync(CancellationToken ct = default);
        Task<ClienteEntity> AddAsync(ClienteEntity entity, CancellationToken ct = default);
        Task UpdateAsync(ClienteEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default);
        Task<ClienteEntity?> GetByIdentificacionAsync(string tipo, string numero, CancellationToken ct = default);
        Task<ClienteEntity?> GetByCorreoAsync(string correo, CancellationToken ct = default);
        Task<bool> ExistsByIdentificacionAsync(string tipo, string numero, CancellationToken ct = default);
        Task<bool> ExistsByCorreoAsync(string correo, int? excludeId = null, CancellationToken ct = default);
    }
}