using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using Servicio.Hotel.DataManagement.Hospedaje.Models;
using Servicio.Hotel.DataManagement.Common;

namespace Servicio.Hotel.DataManagement.Hospedaje.Interfaces
{
    public interface IEstadiaDataService
    {
        Task<EstadiaDataModel> GetByIdAsync(int id, CancellationToken ct = default);
        Task<EstadiaDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<DataPagedResult<EstadiaDataModel>> GetByFiltroAsync(EstadiaFiltroDataModel filtro, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<EstadiaDataModel> AddAsync(EstadiaDataModel model, CancellationToken ct = default);
        Task UpdateAsync(EstadiaDataModel model, CancellationToken ct = default);

        Task RegistrarCheckoutAsync(int idEstadia, string observaciones, bool requiereMantenimiento, string usuario, CancellationToken ct = default);
        Task<int> HacerCheckinAsync(int idReserva, string usuario, CancellationToken ct = default);
        Task<IEnumerable<EstadiaDataModel>> GetByReservaAsync(int idReserva, CancellationToken ct = default);

        // Cargos de estadía (operaciones anidadas)
        Task<CargoEstadiaDataModel> AddCargoAsync(int idEstadia, CargoEstadiaDataModel cargo, CancellationToken ct = default);
        Task<CargoEstadiaDataModel?> GetCargoByIdAsync(int idCargo, CancellationToken ct = default);
        Task AnularCargoAsync(int idCargo, string usuario, CancellationToken ct = default);
        Task<DataPagedResult<CargoEstadiaDataModel>> GetCargosByEstadiaAsync(int idEstadia, int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
