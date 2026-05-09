using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento
{
    public interface ISucursalRepository
    {
        // CRUD básico
        Task<SucursalEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<SucursalEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<SucursalEntity>> GetAllAsync(CancellationToken ct = default);
        Task<SucursalEntity> AddAsync(SucursalEntity entity, CancellationToken ct = default);
        Task UpdateAsync(SucursalEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task UpdatePoliticasAsync(int id, SucursalEntity politicas, CancellationToken ct = default);
        Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default);
        Task<SucursalEntity?> GetByCodigoAsync(string codigo, CancellationToken ct = default);
    }
}