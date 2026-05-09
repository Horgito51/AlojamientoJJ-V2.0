using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataManagement.Alojamiento.Models;
using Servicio.Hotel.DataManagement.Common;

namespace Servicio.Hotel.DataManagement.Alojamiento.Interfaces
{
    public interface ITarifaDataService
    {
        Task<TarifaDataModel> GetByIdAsync(int id, CancellationToken ct = default);
        Task<TarifaDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<DataPagedResult<TarifaDataModel>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<TarifaDataModel> AddAsync(TarifaDataModel model, CancellationToken ct = default);
        Task UpdateAsync(TarifaDataModel model, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<TarifaDataModel> GetTarifaVigenteAsync(int idSucursal, int idTipoHabitacion, DateTime fecha, string? canal = null, CancellationToken ct = default);
        Task<TarifaDataModel> GetTarifaVigenteRangoAsync(int idSucursal, int idTipoHabitacion, DateTime fechaInicio, DateTime fechaFin, string? canal = null, CancellationToken ct = default);
        Task<IEnumerable<TarifaDataModel>> GetBySucursalAsync(int idSucursal, CancellationToken ct = default);
        Task DesactivarAsync(int id, string usuario, CancellationToken ct = default);
    }
}
