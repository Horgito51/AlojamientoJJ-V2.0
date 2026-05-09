using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Facturacion;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Facturacion
{
    public interface IFacturaRepository
    {
        // CRUD básico
        Task<FacturaEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<FacturaEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<FacturaEntity>> GetAllAsync(CancellationToken ct = default);
        Task<FacturaEntity> AddAsync(FacturaEntity entity, CancellationToken ct = default);
        Task UpdateAsync(FacturaEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task UpdateSaldoPendienteAsync(int idFactura, decimal nuevoSaldo, CancellationToken ct = default);
        Task AnularAsync(int idFactura, string motivo, string usuario, CancellationToken ct = default);
        Task<bool> EstaPagadaAsync(int idFactura, CancellationToken ct = default);

        // Métodos para generar facturas (ejecutan SP)
        Task<int> GenerarFacturaReservaAsync(int idReserva, string usuario, CancellationToken ct = default);
        Task<int> GenerarFacturaFinalAsync(int idReserva, string usuario, CancellationToken ct = default);
    }
}