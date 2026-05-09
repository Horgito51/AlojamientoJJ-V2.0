using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataManagement.Reservas.Models;
using Servicio.Hotel.DataManagement.Common;

namespace Servicio.Hotel.DataManagement.Reservas.Interfaces
{
    public interface IReservaDataService
    {
        Task<ReservaDataModel> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ReservaDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<ReservaDataModel> GetByCodigoAsync(string codigo, CancellationToken ct = default);
        Task<DataPagedResult<ReservaDataModel>> GetByFiltroAsync(ReservaFiltroDataModel filtro, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<ReservaDataModel> AddAsync(ReservaDataModel model, CancellationToken ct = default);
        Task UpdateAsync(ReservaDataModel model, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task ConfirmarAsync(int idReserva, string usuario, CancellationToken ct = default);
        Task CancelarAsync(int idReserva, string motivo, string usuario, CancellationToken ct = default);
        Task FinalizarAsync(int idReserva, string usuario, CancellationToken ct = default);
        Task<bool> PuedeCancelarAsync(int idReserva, CancellationToken ct = default);
        Task<int> ConfirmarReservaHabitacionAsync(int idReserva, int idHabitacion, int? idTarifa, DateTime fechaInicio, DateTime fechaFin, int numAdultos, int numNinos, decimal precioNoche, string usuario, CancellationToken ct = default);
        Task<bool> ExisteSolapamientoAsync(int idHabitacion, DateTime fechaInicio, DateTime fechaFin, int? excludeIdReserva = null, CancellationToken ct = default);
    }
}
