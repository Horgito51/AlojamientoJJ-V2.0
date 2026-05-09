using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento
{
    public interface IHabitacionRepository
    {
        // CRUD básico
        Task<HabitacionEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<HabitacionEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<HabitacionEntity>> GetAllAsync(CancellationToken ct = default);
        Task<HabitacionEntity> AddAsync(HabitacionEntity entity, CancellationToken ct = default);
        Task UpdateAsync(HabitacionEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task UpdateEstadoAsync(int id, string nuevoEstado, string usuario, CancellationToken ct = default);
        Task<bool> ExistsByNumeroEnSucursalAsync(int idSucursal, string numero, CancellationToken ct = default);

        // Nuevos métodos para consultas
        Task<IEnumerable<HabitacionEntity>> GetBySucursalAsync(int idSucursal, CancellationToken ct = default);
        Task<IEnumerable<HabitacionEntity>> GetByTipoHabitacionAsync(int idTipoHabitacion, CancellationToken ct = default);
        Task<IEnumerable<HabitacionEntity>> GetDisponiblesAsync(int idSucursal, DateTime inicio, DateTime fin, CancellationToken ct = default);
    }
}