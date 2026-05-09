using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Facturacion;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Facturacion;
using Servicio.Hotel.Business.Mappers.Facturacion;
using Servicio.Hotel.Business.Validators.Facturacion;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataManagement.Facturacion.Interfaces;

namespace Servicio.Hotel.Business.Services.Facturacion
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaDataService _facturaDataService;
        private readonly ServicioHotelDbContext _context;

        public FacturaService(IFacturaDataService facturaDataService, ServicioHotelDbContext context)
        {
            _facturaDataService = facturaDataService;
            _context = context;
        }

        public async Task<FacturaDTO> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var dataModel = await _facturaDataService.GetByIdAsync(id, ct);
            if (dataModel == null)
                throw new NotFoundException("FAC-001", $"No se encontró la factura con ID {id}.");
            return dataModel.ToDto();
        }

        public async Task<FacturaDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var dataModel = await _facturaDataService.GetByGuidAsync(guid, ct);
            if (dataModel == null)
                throw new NotFoundException("FAC-002", $"No se encontró la factura con GUID {guid}.");
            return dataModel.ToDto();
        }

        public async Task<PagedResult<FacturaDTO>> GetByFiltroAsync(FacturaFiltroDTO filtro, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var pagedData = await _facturaDataService.GetByFiltroAsync(filtro.ToDataModel(), pageNumber, pageSize, ct);
            return new PagedResult<FacturaDTO>
            {
                Items = pagedData.Items.ToDtoList(),
                TotalCount = pagedData.TotalCount,
                PageNumber = pagedData.PageNumber,
                PageSize = pagedData.PageSize
            };
        }

        public async Task<FacturaDTO> CreateAsync(FacturaDTO facturaDto, CancellationToken ct = default)
        {
            FacturaValidator.Validate(facturaDto);
            var dataModel = facturaDto.ToDataModel();
            var created = await _facturaDataService.AddAsync(dataModel, ct);
            return created.ToDto();
        }

        public async Task UpdateAsync(FacturaDTO facturaDto, CancellationToken ct = default)
        {
            FacturaValidator.Validate(facturaDto);
            var existing = await _facturaDataService.GetByIdAsync(facturaDto.IdFactura, ct);
            if (existing == null)
                throw new NotFoundException("FAC-003", $"No se encontró la factura con ID {facturaDto.IdFactura}.");
            var dataModel = facturaDto.ToDataModel();
            await _facturaDataService.UpdateAsync(dataModel, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _facturaDataService.GetByIdAsync(id, ct);
            if (existing == null)
                throw new NotFoundException("FAC-004", $"No se encontró la factura con ID {id}.");
            await _facturaDataService.DeleteAsync(id, ct);
        }

        public async Task UpdateSaldoPendienteAsync(int idFactura, decimal nuevoSaldo, CancellationToken ct = default)
        {
            var existing = await _facturaDataService.GetByIdAsync(idFactura, ct);
            if (existing == null)
                throw new NotFoundException("FAC-005", $"No se encontró la factura con ID {idFactura}.");
            await _facturaDataService.UpdateSaldoPendienteAsync(idFactura, nuevoSaldo, ct);
        }

        public async Task AnularAsync(int idFactura, string motivo, string usuario, CancellationToken ct = default)
        {
            var existing = await _facturaDataService.GetByIdAsync(idFactura, ct);
            if (existing == null)
                throw new NotFoundException("FAC-006", $"No se encontró la factura con ID {idFactura}.");
            await _facturaDataService.AnularAsync(idFactura, motivo, usuario, ct);
        }

        public async Task<int> GenerarFacturaReservaAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            var reserva = await _context.Reservas.FindAsync(new object[] { idReserva }, ct);
            if (reserva == null)
                throw new NotFoundException("FAC-007", $"No se encontro la reserva con ID {idReserva}.");

            if (reserva.EstadoReserva != "CON")
                throw new ValidationException("FAC-008", "Solo se puede generar factura para una reserva confirmada.");

            var filtro = new FacturaFiltroDTO
            {
                IdReserva = idReserva,
                TipoFactura = "RESERVA",
                EsEliminado = false
            };

            var existentes = await _facturaDataService.GetByFiltroAsync(filtro.ToDataModel(), 1, int.MaxValue, ct);
            if (existentes.Items != null && existentes.Items.Any(f => f.TipoFactura == "RESERVA" && f.Estado != "ANU" && !f.EsEliminado))
                throw new ConflictException("Ya existe una factura generada para esta reserva.");

            return await _facturaDataService.GenerarFacturaReservaAsync(idReserva, usuario, ct);
        }

        public async Task<int> GenerarFacturaFinalAsync(int idReserva, string usuario, CancellationToken ct = default)
        {
            return await _facturaDataService.GenerarFacturaFinalAsync(idReserva, usuario, ct);
        }
    }
}
