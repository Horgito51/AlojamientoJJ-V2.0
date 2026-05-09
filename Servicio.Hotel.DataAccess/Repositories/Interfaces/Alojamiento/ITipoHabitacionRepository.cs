using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento
{
    public interface ITipoHabitacionRepository
    {
        // CRUD básico
        Task<TipoHabitacionEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<TipoHabitacionEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<TipoHabitacionEntity?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<IEnumerable<TipoHabitacionEntity>> GetAllAsync(CancellationToken ct = default);
        Task<TipoHabitacionEntity> AddAsync(TipoHabitacionEntity entity, CancellationToken ct = default);
        Task UpdateAsync(TipoHabitacionEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken ct = default);
        Task<bool> ExistsBySlugAsync(string slug, CancellationToken ct = default);

        // Nuevo método
        Task<IEnumerable<TipoHabitacionEntity>> GetPublicosAsync(CancellationToken ct = default);
    }
}
