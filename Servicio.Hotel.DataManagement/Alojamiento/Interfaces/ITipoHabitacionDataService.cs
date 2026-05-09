using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataManagement.Alojamiento.Models;
using Servicio.Hotel.DataManagement.Common;

namespace Servicio.Hotel.DataManagement.Alojamiento.Interfaces
{
    public interface ITipoHabitacionDataService
    {
        Task<TipoHabitacionDataModel> GetByIdAsync(int id, CancellationToken ct = default);
        Task<TipoHabitacionDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<TipoHabitacionDataModel> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<DataPagedResult<TipoHabitacionDataModel>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<TipoHabitacionDataModel> AddAsync(TipoHabitacionDataModel model, CancellationToken ct = default);
        Task UpdateAsync(TipoHabitacionDataModel model, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<IEnumerable<TipoHabitacionDataModel>> GetPublicosAsync(CancellationToken ct = default);
        Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken ct = default);
        Task<bool> ExistsBySlugAsync(string slug, CancellationToken ct = default);
    }
}
