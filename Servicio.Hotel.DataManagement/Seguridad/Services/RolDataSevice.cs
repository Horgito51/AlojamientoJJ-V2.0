using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Seguridad;
using Servicio.Hotel.DataManagement.Seguridad.Interfaces;
using Servicio.Hotel.DataManagement.Seguridad.Models;
using Servicio.Hotel.DataManagement.Seguridad.Mappers;
using Servicio.Hotel.DataManagement.Common;
using Servicio.Hotel.DataManagement.UnitOfWork;

namespace Servicio.Hotel.DataManagement.Seguridad.Services
{
    public class RolDataService : IRolDataService
    {
        private readonly IRolRepository _rolRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RolDataService(IRolRepository rolRepository, IUnitOfWork unitOfWork)
        {
            _rolRepository = rolRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RolDataModel> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _rolRepository.GetByIdAsync(id, ct);
            return entity?.ToModel();
        }

        public async Task<RolDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var entity = await _rolRepository.GetByGuidAsync(guid, ct);
            return entity?.ToModel();
        }

        public async Task<DataPagedResult<RolDataModel>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var entities = await _rolRepository.GetAllAsync(ct);
            var items = entities.ToModelList();
            var totalCount = items.Count;
            var pagedItems = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new DataPagedResult<RolDataModel>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<RolDataModel> AddAsync(RolDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            if (entity.RolGuid == Guid.Empty) entity.RolGuid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.CreadoPorUsuario)) entity.CreadoPorUsuario = "Sistema";
            entity.FechaRegistroUtc = DateTime.UtcNow;
            var added = await _rolRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return added.ToModel();
        }

        public async Task UpdateAsync(RolDataModel model, CancellationToken ct = default)
        {
            var entity = await _rolRepository.GetByIdAsync(model.IdRol, ct);
            if (entity == null) return;

            // Actualizamos los campos que pueden cambiar
            entity.NombreRol = model.NombreRol;
            entity.DescripcionRol = model.DescripcionRol;
            entity.EstadoRol = model.EstadoRol;
            entity.Activo = model.Activo;
            entity.ModificadoPorUsuario = model.ModificadoPorUsuario ?? "Sistema";
            entity.FechaModificacionUtc = DateTime.UtcNow;

            await _rolRepository.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _rolRepository.DeleteAsync(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<RolDataModel> GetByNombreAsync(string nombre, CancellationToken ct = default)
        {
            var entity = await _rolRepository.GetByNombreAsync(nombre, ct);
            return entity?.ToModel();
        }

        public async Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default)
        {
            await _rolRepository.InhabilitarAsync(id, motivo, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null, CancellationToken ct = default)
        {
            return await _rolRepository.ExistsByNombreAsync(nombre, excludeId, ct);
        }
    }
}