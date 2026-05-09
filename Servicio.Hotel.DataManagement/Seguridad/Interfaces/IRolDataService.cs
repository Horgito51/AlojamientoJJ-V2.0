using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataManagement.Seguridad.Models;
using Servicio.Hotel.DataManagement.Common;

namespace Servicio.Hotel.DataManagement.Seguridad.Interfaces
{
    public interface IRolDataService
    {
        Task<RolDataModel> GetByIdAsync(int id, CancellationToken ct = default);
        Task<RolDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<DataPagedResult<RolDataModel>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<RolDataModel> AddAsync(RolDataModel model, CancellationToken ct = default);
        Task UpdateAsync(RolDataModel model, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<RolDataModel> GetByNombreAsync(string nombre, CancellationToken ct = default);
        Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default);
        Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null, CancellationToken ct = default);
    }
}