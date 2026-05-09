using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataManagement.Alojamiento.Models;

namespace Servicio.Hotel.DataManagement.Alojamiento.Interfaces
{
    public interface ICatalogoServicioDataService
    {
        Task<CatalogoServicioDataModel?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<CatalogoServicioDataModel?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<CatalogoServicioDataModel>> GetAllAsync(CancellationToken ct = default);
        Task<CatalogoServicioDataModel> AddAsync(CatalogoServicioDataModel model, CancellationToken ct = default);
        Task UpdateAsync(CatalogoServicioDataModel model, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
