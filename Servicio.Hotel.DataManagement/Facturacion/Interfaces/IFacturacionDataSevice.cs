using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataManagement.Facturacion.Models;
using Servicio.Hotel.DataManagement.Common;

namespace Servicio.Hotel.DataManagement.Facturacion.Interfaces
{
    public interface IFacturaDataService
    {
        Task<FacturaDataModel> GetByIdAsync(int id, CancellationToken ct = default);
        Task<FacturaDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<DataPagedResult<FacturaDataModel>> GetByFiltroAsync(FacturaFiltroDataModel filtro, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<FacturaDataModel> AddAsync(FacturaDataModel model, CancellationToken ct = default);
        Task UpdateAsync(FacturaDataModel model, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task UpdateSaldoPendienteAsync(int idFactura, decimal nuevoSaldo, CancellationToken ct = default);
        Task AnularAsync(int idFactura, string motivo, string usuario, CancellationToken ct = default);
        Task<int> GenerarFacturaReservaAsync(int idReserva, string usuario, CancellationToken ct = default);
        Task<int> GenerarFacturaFinalAsync(int idReserva, string usuario, CancellationToken ct = default);
    }
}