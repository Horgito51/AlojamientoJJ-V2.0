using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Facturacion;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Facturacion
{
    public interface IFacturaDetalleRepository
    {
        // CRUD básico (solo lectura, el detalle es inmutable)
        Task<FacturaDetalleEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<FacturaDetalleEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<FacturaDetalleEntity>> GetByFacturaAsync(int idFactura, CancellationToken ct = default);

        // Solo para creación automática
        Task<FacturaDetalleEntity> AddAsync(FacturaDetalleEntity entity, CancellationToken ct = default);
    }
}