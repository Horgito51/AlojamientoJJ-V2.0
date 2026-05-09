using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento
{
    public interface ITipoHabitacionCatalogoRepository
    {
        // CRUD básico
        Task<TipoHabitacionCatalogoEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<TipoHabitacionCatalogoEntity>> GetAllAsync(CancellationToken ct = default);
        Task<TipoHabitacionCatalogoEntity> AddAsync(TipoHabitacionCatalogoEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task DeleteByTipoAndCatalogoAsync(int idTipoHabitacion, int idCatalogo, CancellationToken ct = default);
        Task<bool> ExistsAsync(int idTipoHabitacion, int idCatalogo, CancellationToken ct = default);
    }
}