using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataManagement.Reservas.Models;
using Servicio.Hotel.DataManagement.Common;

namespace Servicio.Hotel.DataManagement.Reservas.Interfaces
{
    public interface IClienteDataService
    {
        Task<ClienteDataModel> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ClienteDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<DataPagedResult<ClienteDataModel>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<ClienteDataModel> AddAsync(ClienteDataModel model, CancellationToken ct = default);
        Task UpdateAsync(ClienteDataModel model, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<ClienteDataModel> GetByIdentificacionAsync(string tipo, string numero, CancellationToken ct = default);
        Task<ClienteDataModel> GetByCorreoAsync(string correo, CancellationToken ct = default);
        Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default);
    }
}