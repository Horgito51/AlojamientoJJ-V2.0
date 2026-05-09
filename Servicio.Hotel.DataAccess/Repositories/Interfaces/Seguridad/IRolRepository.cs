using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Seguridad;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Seguridad
{
    public interface IRolRepository
    {
        // CRUD básico
        Task<RolEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<RolEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<RolEntity>> GetAllAsync(CancellationToken ct = default);
        Task<RolEntity> AddAsync(RolEntity entity, CancellationToken ct = default);
        Task UpdateAsync(RolEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default);
        Task<RolEntity?> GetByNombreAsync(string nombre, CancellationToken ct = default);
        Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null, CancellationToken ct = default);
    }
}