using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Repositories.Interfaces.Valoraciones;
using Servicio.Hotel.DataManagement.Valoraciones.Interfaces;
using Servicio.Hotel.DataManagement.Valoraciones.Models;
using Servicio.Hotel.DataManagement.Valoraciones.Mappers;
using Servicio.Hotel.DataManagement.Common;
using Servicio.Hotel.DataManagement.UnitOfWork;

namespace Servicio.Hotel.DataManagement.Valoraciones.Services
{
    public class ValoracionDataService : IValoracionDataService
    {
        private readonly IValoracionRepository _valoracionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ValoracionDataService(IValoracionRepository valoracionRepository, IUnitOfWork unitOfWork)
        {
            _valoracionRepository = valoracionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ValoracionDataModel> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _valoracionRepository.GetByIdAsync(id, ct);
            return entity?.ToModel();
        }

        public async Task<ValoracionDataModel> GetByGuidAsync(Guid guid, CancellationToken ct = default)
        {
            var entity = await _valoracionRepository.GetByGuidAsync(guid, ct);
            return entity?.ToModel();
        }

        public async Task<DataPagedResult<ValoracionDataModel>> GetByFiltroAsync(ValoracionFiltroDataModel filtro, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            var all = await _valoracionRepository.GetAllAsync(ct);
            var query = all.AsQueryable();

            if (filtro.IdSucursal.HasValue)
                query = query.Where(v => v.IdSucursal == filtro.IdSucursal.Value);
            if (filtro.IdCliente.HasValue)
                query = query.Where(v => v.IdCliente == filtro.IdCliente.Value);
            if (!string.IsNullOrEmpty(filtro.EstadoValoracion))
                query = query.Where(v => v.EstadoValoracion == filtro.EstadoValoracion);
            if (!string.IsNullOrEmpty(filtro.TipoViaje))
                query = query.Where(v => v.TipoViaje == filtro.TipoViaje);
            if (filtro.PublicadaEnPortal.HasValue)
                query = query.Where(v => v.PublicadaEnPortal == filtro.PublicadaEnPortal.Value);
            if (filtro.PuntuacionMin.HasValue)
                query = query.Where(v => v.PuntuacionGeneral >= filtro.PuntuacionMin.Value);
            if (filtro.PuntuacionMax.HasValue)
                query = query.Where(v => v.PuntuacionGeneral <= filtro.PuntuacionMax.Value);
            if (filtro.FechaDesde.HasValue)
                query = query.Where(v => v.FechaRegistroUtc >= filtro.FechaDesde.Value);
            if (filtro.FechaHasta.HasValue)
                query = query.Where(v => v.FechaRegistroUtc <= filtro.FechaHasta.Value);

            var totalCount = query.Count();
            var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new DataPagedResult<ValoracionDataModel>
            {
                Items = items.ToModelList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ValoracionDataModel> AddAsync(ValoracionDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            if (entity.ValoracionGuid == Guid.Empty) entity.ValoracionGuid = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(entity.CreadoPorUsuario)) entity.CreadoPorUsuario = "Sistema";
            if (string.IsNullOrWhiteSpace(entity.ServicioOrigen)) entity.ServicioOrigen = "reputacion-service";
            entity.FechaRegistroUtc = DateTime.UtcNow;
            var added = await _valoracionRepository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return added.ToModel();
        }

        public async Task UpdateAsync(ValoracionDataModel model, CancellationToken ct = default)
        {
            var entity = model.ToEntity();
            await _valoracionRepository.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _valoracionRepository.DeleteAsync(id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task ModerarAsync(int idValoracion, string nuevoEstado, string motivo, string moderador, CancellationToken ct = default)
        {
            await _valoracionRepository.ModerarAsync(idValoracion, nuevoEstado, motivo, moderador, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task ResponderAsync(int idValoracion, string respuesta, string usuario, CancellationToken ct = default)
        {
            await _valoracionRepository.ResponderAsync(idValoracion, respuesta, usuario, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsByEstadiaAsync(int idEstadia, CancellationToken ct = default)
        {
            return await _valoracionRepository.ExistsByEstadiaAsync(idEstadia, ct);
        }
    }
}