using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Hospedaje;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Hospedaje
{
    public interface IEstadiaRepository
    {
        // CRUD básico
        Task<EstadiaEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<EstadiaEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<EstadiaEntity>> GetAllAsync(CancellationToken ct = default);
        Task<EstadiaEntity> AddAsync(EstadiaEntity entity, CancellationToken ct = default);
        Task UpdateAsync(EstadiaEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task RegistrarCheckoutAsync(int idEstadia, string observaciones, bool requiereMantenimiento, string usuario, CancellationToken ct = default);
        Task<bool> EstaFinalizadaAsync(int idEstadia, CancellationToken ct = default);

        // Método para check-in (ejecuta SP)
        Task<int> HacerCheckinAsync(int idReserva, string usuario, CancellationToken ct = default);

        // Consultas auxiliares
        Task<IEnumerable<EstadiaEntity>> GetByReservaAsync(int idReserva, CancellationToken ct = default);
    }
}
