using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.Business.Exceptions;
using Servicio.Hotel.Business.Interfaces.Reservas;
using Servicio.Hotel.Business.Mappers.Reservas;
using Servicio.Hotel.Business.Validators.Reservas;
using Servicio.Hotel.DataAccess.Context;
using Servicio.Hotel.DataManagement.Reservas.Interfaces;

namespace Servicio.Hotel.Business.Services.Reservas
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteDataService _clienteDataService;
        private readonly ServicioHotelDbContext _context;

        public ClienteService(IClienteDataService clienteDataService, ServicioHotelDbContext context)
        {
            _clienteDataService = clienteDataService;
            _context = context;
        }

        public async Task<ClienteDTO> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var dataModel = await _clienteDataService.GetByIdAsync(id, ct);
            if (dataModel == null)
                throw new NotFoundException("CLI-001", $"No se encontró el cliente con ID {id}.");
            return dataModel.ToDto();
        }

        public async Task<ClienteDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var dataModel = await _clienteDataService.GetByGuidAsync(guid, ct);
            if (dataModel == null)
                throw new NotFoundException("CLI-002", $"No se encontró el cliente con GUID {guid}.");
            return dataModel.ToDto();
        }

        public async Task<PagedResult<ClienteDTO>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var pagedData = await _clienteDataService.GetAllPagedAsync(pageNumber, pageSize, ct);
            return new PagedResult<ClienteDTO>
            {
                Items = pagedData.Items.ToDtoList(),
                TotalCount = pagedData.TotalCount,
                PageNumber = pagedData.PageNumber,
                PageSize = pagedData.PageSize
            };
        }

        public async Task<ClienteDTO> CreateAsync(ClienteCreateDTO clienteCreateDto, CancellationToken ct = default)
        {
            var clienteDto = new ClienteDTO
            {
                TipoIdentificacion = clienteCreateDto.TipoIdentificacion,
                NumeroIdentificacion = clienteCreateDto.NumeroIdentificacion,
                Nombres = clienteCreateDto.Nombres,
                Correo = clienteCreateDto.Correo,
                Telefono = clienteCreateDto.Telefono ?? string.Empty
            };

            ClienteValidator.Validate(clienteDto);

            var identificacionExistente = await _clienteDataService.GetByIdentificacionAsync(
                clienteCreateDto.TipoIdentificacion,
                clienteCreateDto.NumeroIdentificacion,
                ct);
            if (identificacionExistente != null)
                throw new ValidationException("CLI-009", "Ya existe un cliente registrado con esa identificacion.");

            // Verificar correo duplicado antes de llegar a la BD
            var existente = await _clienteDataService.GetByCorreoAsync(clienteCreateDto.Correo, ct);
            if (existente != null)
                throw new ValidationException("CLI-010", $"Ya existe un cliente registrado con el correo '{clienteCreateDto.Correo}'.");

            var dataModel = clienteCreateDto.ToDataModel();
            var created = await _clienteDataService.AddAsync(dataModel, ct);
            return created.ToDto();
        }

        public async Task UpdateAsync(ClienteUpdateDTO clienteUpdateDto, CancellationToken ct = default)
        {
            var existing = await _clienteDataService.GetByIdAsync(clienteUpdateDto.IdCliente, ct);
            if (existing == null)
                throw new NotFoundException("CLI-003", $"No se encontró el cliente con ID {clienteUpdateDto.IdCliente}.");

            ClienteValidator.Validate(new ClienteDTO
            {
                IdCliente = existing.IdCliente,
                TipoIdentificacion = existing.TipoIdentificacion,
                NumeroIdentificacion = existing.NumeroIdentificacion,
                Nombres = clienteUpdateDto.Nombres,
                Apellidos = clienteUpdateDto.Apellidos,
                RazonSocial = clienteUpdateDto.RazonSocial ?? string.Empty,
                Correo = clienteUpdateDto.Correo,
                Telefono = clienteUpdateDto.Telefono ?? string.Empty,
                Estado = clienteUpdateDto.Estado
            });

            var correoExistente = await _clienteDataService.GetByCorreoAsync(clienteUpdateDto.Correo, ct);
            if (correoExistente != null && correoExistente.IdCliente != clienteUpdateDto.IdCliente)
                throw new ValidationException("CLI-011", $"Ya existe un cliente registrado con el correo '{clienteUpdateDto.Correo}'.");

            existing.Nombres = clienteUpdateDto.Nombres;
            existing.Apellidos = clienteUpdateDto.Apellidos;
            existing.RazonSocial = clienteUpdateDto.RazonSocial ?? string.Empty;
            existing.Correo = clienteUpdateDto.Correo;
            existing.Telefono = clienteUpdateDto.Telefono ?? string.Empty;
            existing.Direccion = clienteUpdateDto.Direccion ?? string.Empty;
            existing.Estado = clienteUpdateDto.Estado;

            await _clienteDataService.UpdateAsync(existing, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _clienteDataService.GetByIdAsync(id, ct);
            if (existing == null)
                throw new NotFoundException("CLI-004", $"No se encontró el cliente con ID {id}.");
            await EnsureSinReservasActivasAsync(id, ct);
            await _clienteDataService.DeleteAsync(id, ct);
        }

        public async Task<ClienteDTO> GetByIdentificacionAsync(string tipo, string numero, CancellationToken ct = default)
        {
            var dataModel = await _clienteDataService.GetByIdentificacionAsync(tipo, numero, ct);
            if (dataModel == null)
                throw new NotFoundException("CLI-005", $"No se encontró cliente con identificación {tipo} {numero}.");
            return dataModel.ToDto();
        }

        public async Task<ClienteDTO> GetByCorreoAsync(string correo, CancellationToken ct = default)
        {
            var dataModel = await _clienteDataService.GetByCorreoAsync(correo, ct);
            if (dataModel == null)
                throw new NotFoundException("CLI-006", $"No se encontró cliente con correo {correo}.");
            return dataModel.ToDto();
        }

        public async Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default)
        {
            var existing = await _clienteDataService.GetByIdAsync(id, ct);
            if (existing == null)
                throw new NotFoundException("CLI-007", $"No se encontró el cliente con ID {id}.");
            await EnsureSinReservasActivasAsync(id, ct);
            await _clienteDataService.InhabilitarAsync(id, motivo, usuario, ct);
        }

        private async Task EnsureSinReservasActivasAsync(int idCliente, CancellationToken ct)
        {
            var estadosActivos = new[] { "PEN", "CON" };
            var tieneReservasActivas = await _context.Reservas.AnyAsync(r =>
                r.IdCliente == idCliente && estadosActivos.Contains(r.EstadoReserva), ct);

            if (tieneReservasActivas)
                throw new ConflictException("No se puede eliminar o inhabilitar el cliente porque tiene reservas activas asociadas.");
        }
    }
}
