using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Reservas;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Reservas
{
    public interface IReservaHabitacionRepository
    {
        // CRUD básico
        Task<ReservaHabitacionEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ReservaHabitacionEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<ReservaHabitacionEntity>> GetAllAsync(CancellationToken ct = default);
        Task<ReservaHabitacionEntity> AddAsync(ReservaHabitacionEntity entity, CancellationToken ct = default);
        Task UpdateAsync(ReservaHabitacionEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task UpdateEstadoDetalleAsync(int idReservaHabitacion, string nuevoEstado, string usuario, CancellationToken ct = default);

        // Validaciones de negocio
        Task<bool> ExistsSolapamientoAsync(int idHabitacion, DateTime fechaInicio, DateTime fechaFin, int? excludeIdReserva = null, CancellationToken ct = default);
    }
}
