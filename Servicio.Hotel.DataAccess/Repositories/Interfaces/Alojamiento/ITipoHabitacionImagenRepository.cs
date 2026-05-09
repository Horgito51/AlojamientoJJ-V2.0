using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento
{
    public interface ITipoHabitacionImagenRepository
    {
        // CRUD básico
        Task<TipoHabitacionImagenEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<TipoHabitacionImagenEntity>> GetAllAsync(CancellationToken ct = default);
        Task<TipoHabitacionImagenEntity> AddAsync(TipoHabitacionImagenEntity entity, CancellationToken ct = default);
        Task UpdateAsync(TipoHabitacionImagenEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task SetPrincipalAsync(int idTipoHabitacion, int idImagen, CancellationToken ct = default);
        Task ReordenarAsync(int idTipoHabitacion, Dictionary<int, int> ordenes, CancellationToken ct = default);
    }
}