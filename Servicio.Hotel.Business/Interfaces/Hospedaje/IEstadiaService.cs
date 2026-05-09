using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Hospedaje;
using Servicio.Hotel.DataManagement.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.Hotel.Business.Interfaces.Hospedaje
{
    public interface IEstadiaService
    {
        Task<EstadiaDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<EstadiaDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<PagedResult<EstadiaDTO>> GetByFiltroAsync(EstadiaFiltroDTO filtro, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<EstadiaDTO> CreateAsync(EstadiaDTO estadiaDto, CancellationToken ct = default);
        Task UpdateAsync(EstadiaDTO estadiaDto, CancellationToken ct = default);

        Task RegistrarCheckoutAsync(int idEstadia, string observaciones, bool requiereMantenimiento, string usuario, CancellationToken ct = default);
        Task<IEnumerable<EstadiaDTO>> HacerCheckinAsync(int idReserva, string usuario, CancellationToken ct = default);

        // Cargos de estadía
        Task<CargoEstadiaDTO> AddCargoAsync(int idEstadia, CargoEstadiaDTO cargoDto, CancellationToken ct = default);
        Task AnularCargoAsync(int idCargo, string usuario, CancellationToken ct = default);
        Task<PagedResult<CargoEstadiaDTO>> GetCargosByEstadiaAsync(int idEstadia, int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
