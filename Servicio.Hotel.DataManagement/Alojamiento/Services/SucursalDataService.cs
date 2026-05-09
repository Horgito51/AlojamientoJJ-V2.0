using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Alojamiento;
using Servicio.Hotel.DataManagement.Alojamiento.Interfaces;
using Servicio.Hotel.DataManagement.Alojamiento.Mappers;
using Servicio.Hotel.DataManagement.Alojamiento.Models;
using Servicio.Hotel.DataManagement.UnitOfWork;

namespace Servicio.Hotel.DataManagement.Alojamiento.Services
{
    public class SucursalDataService : ISucursalDataService
    {
        private readonly ISucursalRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SucursalDataService(ISucursalRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SucursalDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
            => (await _repository.GetByIdAsync(id, ct)).ToModel();

        public async Task<SucursalDataModel?> GetByGuidAsync(Guid guid, CancellationToken ct = default)
            => (await _repository.GetByGuidAsync(guid, ct)).ToModel();

        public async Task<SucursalDataModel?> GetByCodigoAsync(string codigo, CancellationToken ct = default)
            => (await _repository.GetByCodigoAsync(codigo, ct)).ToModel();

        public async Task<IEnumerable<SucursalDataModel>> GetAllAsync(CancellationToken ct = default)
            => (await _repository.GetAllAsync(ct)).ToModelList();

        public async Task<SucursalDataModel> AddAsync(SucursalDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity()!;
            if (entity.SucursalGuid == Guid.Empty) entity.SucursalGuid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.CreadoPorUsuario)) entity.CreadoPorUsuario = "Sistema";
            if (string.IsNullOrWhiteSpace(entity.ServicioOrigen)) entity.ServicioOrigen = "sucursales-service";
            entity.FechaRegistroUtc = DateTime.UtcNow;
            var added = await _repository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return added.ToModel()!;
        }

        public async Task UpdateAsync(SucursalDataModel model, CancellationToken ct = default)
        {
            var entity = await _repository.GetByIdAsync(model.IdSucursal, ct);
            if (entity == null) return;

            // Actualizamos los campos de la sucursal
            entity.NombreSucursal = model.NombreSucursal;
            entity.DescripcionSucursal = model.DescripcionSucursal;
            entity.Pais = model.Pais;
            entity.Provincia = model.Provincia;
            entity.Ciudad = model.Ciudad;
            entity.Direccion = model.Direccion;
            entity.Telefono = model.Telefono;
            entity.Correo = model.Correo;
            entity.Latitud = model.Latitud;
            entity.Longitud = model.Longitud;
            entity.EstadoSucursal = model.EstadoSucursal;
            entity.ModificadoPorUsuario = model.ModificadoPorUsuario ?? "Sistema";
            entity.FechaModificacionUtc = DateTime.UtcNow;

            await _repository.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task UpdatePoliticasAsync(int id, SucursalDataModel politicas, CancellationToken ct = default)
        {
            var entity = politicas.ToEntity()!;
            entity.ModificadoPorUsuario = politicas.ModificadoPorUsuario ?? "Sistema";
            await _repository.UpdatePoliticasAsync(id, entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default)
        {
            await _repository.InhabilitarAsync(id, motivo, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _repository.DeleteAsync(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
