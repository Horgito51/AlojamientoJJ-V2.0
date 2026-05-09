using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Facturacion;
using Servicio.Hotel.DataManagement.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.Hotel.Business.Interfaces.Facturacion
{
    public interface IPagoService
    {
        Task<PagoDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PagoDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<PagedResult<PagoDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<PagedResult<PagoDTO>> GetByFacturaAsync(int idFactura, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<PagoDTO> CreateAsync(PagoDTO pagoDto, CancellationToken ct = default);
        Task UpdateAsync(PagoDTO pagoDto, CancellationToken ct = default);

        Task UpdateEstadoAsync(int idPago, string nuevoEstado, string usuario, CancellationToken ct = default);
        Task<decimal> GetTotalPagadoPorFacturaAsync(int idFactura, CancellationToken ct = default);
        Task<PagoSimuladoDTO> SimularPagoAsync(int idReserva, decimal? monto, string usuario, string? tokenPago = null, string? referencia = null, CancellationToken ct = default);
    }
}
