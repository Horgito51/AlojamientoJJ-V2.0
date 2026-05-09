using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Facturacion;

namespace Servicio.Hotel.Business.Interfaces.Facturacion
{
    public interface IFacturaService
    {
        Task<FacturaDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<FacturaDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<PagedResult<FacturaDTO>> GetByFiltroAsync(FacturaFiltroDTO filtro, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<FacturaDTO> CreateAsync(FacturaDTO facturaDto, CancellationToken ct = default);
        Task UpdateAsync(FacturaDTO facturaDto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task UpdateSaldoPendienteAsync(int idFactura, decimal nuevoSaldo, CancellationToken ct = default);
        Task AnularAsync(int idFactura, string motivo, string usuario, CancellationToken ct = default);
        Task<int> GenerarFacturaReservaAsync(int idReserva, string usuario, CancellationToken ct = default);
        Task<int> GenerarFacturaFinalAsync(int idReserva, string usuario, CancellationToken ct = default);
    }
}