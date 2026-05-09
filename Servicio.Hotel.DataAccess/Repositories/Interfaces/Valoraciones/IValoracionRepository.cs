using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Valoraciones;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Valoraciones
{
    public interface IValoracionRepository
    {
        // CRUD básico
        Task<ValoracionEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ValoracionEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<ValoracionEntity>> GetAllAsync(CancellationToken ct = default);
        Task<ValoracionEntity> AddAsync(ValoracionEntity entity, CancellationToken ct = default);
        Task UpdateAsync(ValoracionEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task ModerarAsync(int idValoracion, string nuevoEstado, string motivo, string moderador, CancellationToken ct = default);
        Task ResponderAsync(int idValoracion, string respuesta, string usuario, CancellationToken ct = default);
        Task<bool> ExistsByEstadiaAsync(int idEstadia, CancellationToken ct = default);
    }
}