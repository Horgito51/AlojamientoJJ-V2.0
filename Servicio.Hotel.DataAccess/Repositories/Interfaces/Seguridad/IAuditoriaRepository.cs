using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Seguridad;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Seguridad
{
    public interface IAuditoriaRepository
    {
        // Solo lectura
        Task<AuditoriaEntity?> GetByIdAsync(long id, CancellationToken ct = default);
        Task<AuditoriaEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<AuditoriaEntity>> GetAllAsync(CancellationToken ct = default);

        // Creación automática (opcional)
        Task<AuditoriaEntity> AddAsync(AuditoriaEntity entity, CancellationToken ct = default);
    }
}