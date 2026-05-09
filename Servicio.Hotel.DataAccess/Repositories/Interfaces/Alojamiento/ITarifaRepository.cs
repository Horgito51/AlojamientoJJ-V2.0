using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento
{
    public interface ITarifaRepository
    {
        // CRUD básico
        Task<TarifaEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<TarifaEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<TarifaEntity>> GetAllAsync(CancellationToken ct = default);
        Task<TarifaEntity> AddAsync(TarifaEntity entity, CancellationToken ct = default);
        Task UpdateAsync(TarifaEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task DesactivarAsync(int id, string usuario, CancellationToken ct = default);
        Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken ct = default);

        // Nuevos métodos necesarios
        Task<TarifaEntity?> GetTarifaVigenteAsync(int idSucursal, int idTipoHabitacion, DateTime fecha, string? canal = null, CancellationToken ct = default);
        Task<TarifaEntity?> GetTarifaVigenteRangoAsync(int idSucursal, int idTipoHabitacion, DateTime fechaInicio, DateTime fechaFin, string? canal = null, CancellationToken ct = default);
        Task<IEnumerable<TarifaEntity>> GetBySucursalAsync(int idSucursal, CancellationToken ct = default);
    }
}
