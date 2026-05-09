using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Facturacion;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Facturacion
{
    public interface IPagoRepository
    {
        // CRUD básico
        Task<PagoEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PagoEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<PagoEntity>> GetAllAsync(CancellationToken ct = default);
        Task<PagoEntity> AddAsync(PagoEntity entity, CancellationToken ct = default);
        Task UpdateAsync(PagoEntity entity, CancellationToken ct = default);

        // Operaciones de escritura
        Task UpdateEstadoAsync(int idPago, string nuevoEstado, string usuario, CancellationToken ct = default);
        Task<bool> ExistsTransaccionExternaAsync(string transaccion, CancellationToken ct = default);

        // Nuevos métodos
        Task<IEnumerable<PagoEntity>> GetByFacturaAsync(int idFactura, CancellationToken ct = default);
        Task<decimal> GetTotalPagadoPorFacturaAsync(int idFactura, CancellationToken ct = default);
    }
}