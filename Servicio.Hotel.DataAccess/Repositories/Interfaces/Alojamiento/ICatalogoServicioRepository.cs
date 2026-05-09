using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Alojamiento;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento
{
    public interface ICatalogoServicioRepository
    {
        // CRUD básico
        Task<CatalogoServicioEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<CatalogoServicioEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<CatalogoServicioEntity>> GetAllAsync(CancellationToken ct = default);
        Task<CatalogoServicioEntity> AddAsync(CatalogoServicioEntity entity, CancellationToken ct = default);
        Task UpdateAsync(CatalogoServicioEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken ct = default);
    }
}