using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Hospedaje;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Hospedaje
{
    public interface ICargoEstadiaRepository
    {
        // CRUD básico
        Task<CargoEstadiaEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<CargoEstadiaEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<CargoEstadiaEntity>> GetAllAsync(CancellationToken ct = default);
        Task<CargoEstadiaEntity> AddAsync(CargoEstadiaEntity entity, CancellationToken ct = default);
        Task UpdateAsync(CargoEstadiaEntity entity, CancellationToken ct = default);

        // Operaciones de escritura
        Task AnularAsync(int idCargo, string usuario, CancellationToken ct = default);

        // Nuevo método
        Task<IEnumerable<CargoEstadiaEntity>> GetByEstadiaAsync(int idEstadia, CancellationToken ct = default);
    }
}