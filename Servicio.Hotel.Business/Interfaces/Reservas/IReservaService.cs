using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.DataManagement.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.Hotel.Business.Interfaces.Reservas
{
    public interface IReservaService
    {
        Task<ReservaDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ReservaDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<ReservaDTO> GetByCodigoAsync(string codigo, CancellationToken ct = default);
        Task<PagedResult<ReservaDTO>> GetByFiltroAsync(ReservaFiltroDTO filtro, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<ReservaDTO> CreateAsync(ReservaCreateDTO reservaCreateDto, CancellationToken ct = default);
        Task<ReservaDTO> CreateByTipoHabitacionAsync(ReservaPorTipoHabitacionCreateDTO reservaCreateDto, CancellationToken ct = default);
        Task<ReservaPrecioDTO> CalcularPrecioHabitacionAsync(int idHabitacion, DateTime fechaInicio, DateTime fechaFin, string? canal = null, CancellationToken ct = default);
        Task UpdateAsync(ReservaUpdateDTO reservaUpdateDto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task ConfirmarAsync(int idReserva, string usuario, CancellationToken ct = default);
        Task CancelarAsync(int idReserva, string motivo, string usuario, CancellationToken ct = default);
        Task FinalizarAsync(int idReserva, string usuario, CancellationToken ct = default);
        Task<bool> PuedeCancelarAsync(int idReserva, CancellationToken ct = default);
        Task<int> ConfirmarReservaHabitacionAsync(int idReserva, int idHabitacion, int? idTarifa, DateTime fechaInicio, DateTime fechaFin, int numAdultos, int numNinos, decimal precioNoche, string usuario, CancellationToken ct = default);
    }
}
