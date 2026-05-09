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
    public class PagoDataService : IPagoDataService
    {
        private readonly IPagoRepository _pagoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PagoDataService(IPagoRepository pagoRepository, IUnitOfWork unitOfWork)
        {
            _pagoRepository = pagoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagoDataModel> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _pagoRepository.GetByIdAsync(id, ct);
            return entity?.ToModel();
        }

        public async Task<PagoDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var entity = await _pagoRepository.GetByGuidAsync(guid, ct);
            return entity?.ToModel();
        }

        public async Task<DataPagedResult<PagoDataModel>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var entities = await _pagoRepository.GetAllAsync(ct);
            var items = entities
                .OrderByDescending(p => p.FechaPagoUtc)
                .ToModelList();
            var totalCount = items.Count;
            var pagedItems = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new DataPagedResult<PagoDataModel>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<DataPagedResult<PagoDataModel>> GetByFacturaAsync(int idFactura, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var entities = await _pagoRepository.GetByFacturaAsync(idFactura, ct);
            var items = entities.OrderByDescending(p => p.FechaPagoUtc).ToModelList();
            var totalCount = items.Count;
            var pagedItems = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new DataPagedResult<PagoDataModel>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagoDataModel> AddAsync(PagoDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            if (entity.PagoGuid == Guid.Empty) entity.PagoGuid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.CreadoPorUsuario)) entity.CreadoPorUsuario = "Sistema";
            if (string.IsNullOrWhiteSpace(entity.ServicioOrigen)) entity.ServicioOrigen = "pagos-service";
            entity.FechaRegistroUtc = DateTime.UtcNow;
            var added = await _pagoRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return added.ToModel();
        }

        public async Task UpdateAsync(PagoDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            await _pagoRepository.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task UpdateEstadoAsync(int idPago, string nuevoEstado, string usuario, CancellationToken ct = default)
        {
            await _pagoRepository.UpdateEstadoAsync(idPago, nuevoEstado, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<decimal> GetTotalPagadoPorFacturaAsync(int idFactura, CancellationToken ct = default)
        {
            return await _pagoRepository.GetTotalPagadoPorFacturaAsync(idFactura, ct);
        }
    }
}
