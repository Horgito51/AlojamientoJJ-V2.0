using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataManagement.Facturacion.Models;
using Servicio.Hotel.DataManagement.Common;

namespace Servicio.Hotel.DataManagement.Facturacion.Interfaces
{
    public interface IPagoDataService
    {
        Task<PagoDataModel> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PagoDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<DataPagedResult<PagoDataModel>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<DataPagedResult<PagoDataModel>> GetByFacturaAsync(int idFactura, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<PagoDataModel> AddAsync(PagoDataModel model, CancellationToken ct = default);
        Task UpdateAsync(PagoDataModel model, CancellationToken ct = default);

        Task UpdateEstadoAsync(int idPago, string nuevoEstado, string usuario, CancellationToken ct = default);
        Task<decimal> GetTotalPagadoPorFacturaAsync(int idFactura, CancellationToken ct = default);
    }
}
