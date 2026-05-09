using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Reservas;
using Servicio.Hotel.DataManagement.Reservas.Interfaces;
using Servicio.Hotel.DataManagement.Reservas.Models;
using Servicio.Hotel.DataManagement.Reservas.Mappers;
using Servicio.Hotel.DataManagement.Common;
using Servicio.Hotel.DataManagement.UnitOfWork;

namespace Servicio.Hotel.DataManagement.Reservas.Services
{
    public class ClienteDataService : IClienteDataService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClienteDataService(IClienteRepository clienteRepository, IUnitOfWork unitOfWork)
        {
            _clienteRepository = clienteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClienteDataModel> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _clienteRepository.GetByIdAsync(id, ct);
            return entity?.ToModel();
        }

        public async Task<ClienteDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var entity = await _clienteRepository.GetByGuidAsync(guid, ct);
            return entity?.ToModel();
        }

        public async Task<DataPagedResult<ClienteDataModel>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var entities = await _clienteRepository.GetAllAsync(ct);
            var items = entities.ToModelList();
            var totalCount = items.Count;
            var pagedItems = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new DataPagedResult<ClienteDataModel>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ClienteDataModel> AddAsync(ClienteDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            if (entity.ClienteGuid == Guid.Empty) entity.ClienteGuid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.CreadoPorUsuario)) entity.CreadoPorUsuario = "Sistema";
            if (string.IsNullOrWhiteSpace(entity.ServicioOrigen)) entity.ServicioOrigen = "clientes-service";
            entity.FechaRegistroUtc = DateTime.UtcNow;
            var added = await _clienteRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return added.ToModel();
        }

        public async Task UpdateAsync(ClienteDataModel model, CancellationToken ct = default)
        {
            var entity = await _clienteRepository.GetByIdAsync(model.IdCliente, ct);
            if (entity == null) return;

            // Actualizamos los campos que pueden cambiar
            entity.Nombres = model.Nombres;
            entity.Apellidos = model.Apellidos;
            entity.RazonSocial = model.RazonSocial;
            entity.Correo = model.Correo;
            entity.Telefono = model.Telefono;
            entity.Direccion = model.Direccion;
            entity.Estado = model.Estado;
            entity.ModificadoPorUsuario = model.ModificadoPorUsuario ?? "Sistema";
            entity.FechaModificacionUtc = DateTime.UtcNow;

            await _clienteRepository.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _clienteRepository.DeleteAsync(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<ClienteDataModel> GetByIdentificacionAsync(string tipo, string numero, CancellationToken ct = default)
        {
            var entity = await _clienteRepository.GetByIdentificacionAsync(tipo, numero, ct);
            return entity?.ToModel();
        }

        public async Task<ClienteDataModel> GetByCorreoAsync(string correo, CancellationToken ct = default)
        {
            var entity = await _clienteRepository.GetByCorreoAsync(correo, ct);
            return entity?.ToModel();
        }

        public async Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default)
        {
            await _clienteRepository.InhabilitarAsync(id, motivo, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}