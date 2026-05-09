using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Facturacion;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Facturacion;
using Servicio.Hotel.Business.Interfaces.Reservas;
using Servicio.Hotel.Business.Mappers.Facturacion;
using Servicio.Hotel.Business.Validators.Facturacion;
using Servicio.Hotel.DataManagement.Facturacion.Interfaces;
using Servicio.Hotel.DataManagement.UnitOfWork;

namespace Servicio.Hotel.Business.Services.Facturacion
{
    public class PagoService : IPagoService
    {
        private static void WriteDebugLog(string hypothesisId, string location, string message, string dataJson)
        {
            try
            {
                var payload = $"{{\"sessionId\":\"86bafb\",\"runId\":\"initial\",\"hypothesisId\":\"{hypothesisId}\",\"location\":\"{location}\",\"message\":\"{message}\",\"data\":{dataJson},\"timestamp\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}}}";
                File.AppendAllText(@"c:\Users\jorge\OneDrive\Escritorio\Puce\Semestre 6\Integracion de sistemas\Practicas\debug-86bafb.log", payload + Environment.NewLine);
            }
            catch
            {
                // ignore debug logger failures
            }
        }

        private readonly IPagoDataService _pagoDataService;
        private readonly IReservaService _reservaService;
        private readonly IFacturaService _facturaService;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IUnitOfWork _unitOfWork;

        public PagoService(
            IPagoDataService pagoDataService,
            IReservaService reservaService,
            IFacturaService facturaService,
            IPaymentGateway paymentGateway,
            IUnitOfWork unitOfWork)
        {
            _pagoDataService = pagoDataService;
            _reservaService = reservaService;
            _facturaService = facturaService;
            _paymentGateway = paymentGateway;
            _unitOfWork = unitOfWork;
        }

        public async Task<PagoDTO> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var dataModel = await _pagoDataService.GetByIdAsync(id, ct);
            if (dataModel == null)
                throw new NotFoundException("PAG-001", $"No se encontró el pago con ID {id}.");
            return dataModel.ToDto();
        }

        public async Task<PagoDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var dataModel = await _pagoDataService.GetByGuidAsync(guid, ct);
            if (dataModel == null)
                throw new NotFoundException("PAG-002", $"No se encontró el pago con GUID {guid}.");
            return dataModel.ToDto();
        }

        public async Task<PagedResult<PagoDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var pagedData = await _pagoDataService.GetAllAsync(pageNumber, pageSize, ct);
            return new PagedResult<PagoDTO>
            {
                Items = pagedData.Items.ToDtoList(),
                TotalCount = pagedData.TotalCount,
                PageNumber = pagedData.PageNumber,
                PageSize = pagedData.PageSize
            };
        }

        public async Task<PagedResult<PagoDTO>> GetByFacturaAsync(int idFactura, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            if (idFactura <= 0)
                throw new ValidationException("PAG-007", "El idFactura es obligatorio para consultar pagos por factura.");

            var pagedData = await _pagoDataService.GetByFacturaAsync(idFactura, pageNumber, pageSize, ct);
            return new PagedResult<PagoDTO>
            {
                Items = pagedData.Items.ToDtoList(),
                TotalCount = pagedData.TotalCount,
                PageNumber = pagedData.PageNumber,
                PageSize = pagedData.PageSize
            };
        }

        public async Task<PagoDTO> CreateAsync(PagoDTO pagoDto, CancellationToken ct = default)
        {
            PagoValidator.Validate(pagoDto);
            var dataModel = pagoDto.ToDataModel();
            var created = await _pagoDataService.AddAsync(dataModel, ct);
            return created.ToDto();
        }

        public async Task UpdateAsync(PagoDTO pagoDto, CancellationToken ct = default)
        {
            var existing = await _pagoDataService.GetByIdAsync(pagoDto.IdPago, ct);
            if (existing == null)
                throw new NotFoundException("PAG-003", $"No se encontró el pago con ID {pagoDto.IdPago}.");
            var dataModel = pagoDto.ToDataModel();
            await _pagoDataService.UpdateAsync(dataModel, ct);
        }

        public async Task UpdateEstadoAsync(int idPago, string nuevoEstado, string usuario, CancellationToken ct = default)
        {
            var existing = await _pagoDataService.GetByIdAsync(idPago, ct);
            if (existing == null)
                throw new NotFoundException("PAG-004", $"No se encontró el pago con ID {idPago}.");
            await _pagoDataService.UpdateEstadoAsync(idPago, nuevoEstado, usuario, ct);
        }

        public async Task<decimal> GetTotalPagadoPorFacturaAsync(int idFactura, CancellationToken ct = default)
        {
            return await _pagoDataService.GetTotalPagadoPorFacturaAsync(idFactura, ct);
        }

        public async Task<PagoSimuladoDTO> SimularPagoAsync(int idReserva, decimal? monto, string usuario, string? tokenPago = null, string? referencia = null, CancellationToken ct = default)
        {
            if (idReserva <= 0)
                throw new ValidationException("PAG-005", "El idReserva es obligatorio.");

            var reserva = await _reservaService.GetByIdAsync(idReserva, ct);
            var facturas = await _facturaService.GetByFiltroAsync(new FacturaFiltroDTO
            {
                IdReserva = idReserva,
                TipoFactura = "RESERVA",
                EsEliminado = false
            }, 1, 10, ct);

            var factura = (facturas?.Items ?? new List<FacturaDTO>())
                .Where(f => f.Estado != "ANU")
                .OrderByDescending(f => f.FechaEmision)
                .FirstOrDefault();

            if (factura == null)
                throw new ValidationException("PAG-008", "La reserva no tiene una factura emitida para registrar el pago.");

            var montoFinal = monto ?? factura.SaldoPendiente;

            if (montoFinal <= 0)
                throw new ValidationException("PAG-006", "El monto del pago debe ser mayor a cero.");

            if (montoFinal > factura.SaldoPendiente)
                throw new ValidationException("PAG-009", "El monto del pago no puede superar el saldo pendiente de la factura.");

            var gatewayResult = await _paymentGateway.ProcesarPagoAsync(new PaymentGatewayRequest
            {
                IdReserva = reserva.IdReserva,
                IdFactura = factura.IdFactura,
                CodigoReserva = reserva.CodigoReserva,
                Monto = montoFinal,
                Moneda = string.IsNullOrWhiteSpace(factura.Moneda) ? "USD" : factura.Moneda,
                Usuario = usuario,
                Referencia = referencia ?? $"RES-{reserva.CodigoReserva}",
                TokenPago = tokenPago
            }, ct);

            if (gatewayResult == null)
                throw new ValidationException("PAG-010", "La pasarela de pago no devolvio un resultado valido.");

            var estadoPago = gatewayResult.Aprobado ? "APR" : "REC";

            try
            {
                // #region agent log
                WriteDebugLog(
                    "H23",
                    "Servicio.Hotel.Business/Services/Facturacion/PagoService.cs:SimularPagoAsync:beforeTransaction",
                    "Before payment transaction persist",
                    $"{{\"idReserva\":{reserva.IdReserva},\"idFactura\":{factura.IdFactura},\"montoFinal\":{montoFinal},\"facturaMoneda\":{(string.IsNullOrWhiteSpace(factura.Moneda) ? "\"USD\"" : $"\\\"{factura.Moneda}\\\"")}}}");
                // #endregion
                await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                await CreateAsync(new PagoDTO
                {
                    IdFactura = factura.IdFactura,
                    IdReserva = reserva.IdReserva,
                    Monto = montoFinal,
                    MetodoPago = "TARJETA",
                    EsPagoElectronico = true,
                    ProveedorPasarela = "EXTERNAL",
                    TransaccionExterna = string.IsNullOrWhiteSpace(gatewayResult.TransaccionExterna)
                        ? $"GW-{Guid.NewGuid():N}"
                        : gatewayResult.TransaccionExterna,
                    CodigoAutorizacion = gatewayResult.CodigoAutorizacion,
                    Referencia = referencia ?? $"RES-{reserva.CodigoReserva}",
                    EstadoPago = estadoPago,
                    FechaPagoUtc = DateTime.UtcNow,
                    Moneda = string.IsNullOrWhiteSpace(factura.Moneda) ? "USD" : factura.Moneda,
                    TipoCambio = 1m,
                    RespuestaPasarela = gatewayResult.RespuestaRaw ?? string.Empty,
                    CreadoPorUsuario = usuario,
                    FechaRegistroUtc = DateTime.UtcNow,
                    ModificadoPorUsuario = string.Empty,
                    ModificacionIp = string.Empty,
                    ServicioOrigen = "facturacion-service"
                }, ct);

                if (gatewayResult.Aprobado)
                {
                    var nuevoSaldoFactura = Math.Max(0, factura.SaldoPendiente - montoFinal);
                    await _facturaService.UpdateSaldoPendienteAsync(factura.IdFactura, nuevoSaldoFactura, ct);

                    await _reservaService.UpdateAsync(new Servicio.Hotel.Business.DTOs.Reservas.ReservaUpdateDTO
                    {
                        IdReserva = reserva.IdReserva,
                        FechaInicio = reserva.FechaInicio,
                        FechaFin = reserva.FechaFin,
                        SubtotalReserva = reserva.SubtotalReserva,
                        ValorIva = reserva.ValorIva,
                        TotalReserva = reserva.TotalReserva,
                        DescuentoAplicado = reserva.DescuentoAplicado,
                        SaldoPendiente = Math.Max(0, reserva.SaldoPendiente - montoFinal),
                        EstadoReserva = "CON",
                        Observaciones = reserva.Observaciones
                    }, ct);
                }
                }, ct);
            }
            catch (Exception ex)
            {
                // #region agent log
                WriteDebugLog(
                    "H24",
                    "Servicio.Hotel.Business/Services/Facturacion/PagoService.cs:SimularPagoAsync:transactionError",
                    "Payment transaction failed",
                    $"{{\"idReserva\":{reserva.IdReserva},\"idFactura\":{factura.IdFactura},\"message\":\"{(ex.Message ?? string.Empty).Replace("\"", "'")}\"}}");
                // #endregion
                throw;
            }

            return new PagoSimuladoDTO
            {
                IdReserva = reserva.IdReserva,
                CodigoReserva = reserva.CodigoReserva,
                Monto = montoFinal,
                EstadoPago = estadoPago,
                EstadoReserva = "CON",
                TransaccionExterna = gatewayResult.TransaccionExterna,
                CodigoAutorizacion = gatewayResult.CodigoAutorizacion,
                Mensaje = gatewayResult.Mensaje,
                FechaPagoUtc = DateTime.UtcNow
            };
        }
    }
}
