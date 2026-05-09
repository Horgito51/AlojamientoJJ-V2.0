using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Facturacion;
using Servicio.Hotel.DataManagement.Facturacion.Interfaces;
using Servicio.Hotel.DataManagement.Facturacion.Models;
using Servicio.Hotel.DataManagement.Facturacion.Mappers;
using Servicio.Hotel.DataManagement.Common;
using Servicio.Hotel.DataManagement.UnitOfWork;

namespace Servicio.Hotel.DataManagement.Facturacion.Services
{
    public class FacturaDataService : IFacturaDataService
    {
        private readonly IFacturaRepository _facturaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FacturaDataService(IFacturaRepository facturaRepository, IUnitOfWork unitOfWork)
        {
            _facturaRepository = facturaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FacturaDataModel> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _facturaRepository.GetByIdAsync(id, ct);
            return entity?.ToModel();
        }

        public async Task<FacturaDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var entity = await _facturaRepository.GetByGuidAsync(guid, ct);
            return entity?.ToModel();
        }

        public async Task<DataPagedResult<FacturaDataModel>> GetByFiltroAsync(FacturaFiltroDataModel filtro, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var all = await _facturaRepository.GetAllAsync(ct);
            var query = all.AsQueryable();

            if (filtro.IdCliente.HasValue)
                query = query.Where(f => f.IdCliente == filtro.IdCliente.Value);
            if (filtro.IdReserva.HasValue)
                query = query.Where(f => f.IdReserva == filtro.IdReserva.Value);
            if (!string.IsNullOrEmpty(filtro.TipoFactura))
                query = query.Where(f => f.TipoFactura == filtro.TipoFactura);
            if (!string.IsNullOrEmpty(filtro.Estado))
                query = query.Where(f => f.Estado == filtro.Estado);
            if (filtro.FechaEmisionDesde.HasValue)
                query = query.Where(f => f.FechaEmision >= filtro.FechaEmisionDesde.Value);
            if (filtro.FechaEmisionHasta.HasValue)
                query = query.Where(f => f.FechaEmision <= filtro.FechaEmisionHasta.Value);
            if (!string.IsNullOrEmpty(filtro.NumeroFactura))
                query = query.Where(f => f.NumeroFactura.Contains(filtro.NumeroFactura));
            if (filtro.EsEliminado.HasValue)
                query = query.Where(f => f.EsEliminado == filtro.EsEliminado.Value);

            var totalCount = query.Count();
            var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new DataPagedResult<FacturaDataModel>
            {
                Items = items.ToModelList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<FacturaDataModel> AddAsync(FacturaDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            if (entity.GuidFactura == Guid.Empty) entity.GuidFactura = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.CreadoPorUsuario)) entity.CreadoPorUsuario = "Sistema";
            if (string.IsNullOrWhiteSpace(entity.ServicioOrigen)) entity.ServicioOrigen = "facturacion-service";
            entity.FechaRegistroUtc = DateTime.UtcNow;
            var added = await _facturaRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return added.ToModel();
        }

        public async Task UpdateAsync(FacturaDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            await _facturaRepository.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _facturaRepository.DeleteAsync(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task UpdateSaldoPendienteAsync(int idFactura, decimal nuevoSaldo, CancellationToken ct = default)
        {
            await _facturaRepository.UpdateSaldoPendienteAsync(idFactura, nuevoSaldo, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task AnularAsync(int idFactura, string motivo, string usuario, CancellationToken ct = default)
        {
            await _facturaRepository.AnularAsync(idFactura, motivo, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<int> GenerarFacturaReservaAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            // Este método ejecuta SP y ya guarda internamente, pero aun así llamamos SaveChanges por si acaso
            var result = await _facturaRepository.GenerarFacturaReservaAsync(idReserva, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return result;
        }

        public async Task<int> GenerarFacturaFinalAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            var result = await _facturaRepository.GenerarFacturaFinalAsync(idReserva, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return result;
        }
    }
}