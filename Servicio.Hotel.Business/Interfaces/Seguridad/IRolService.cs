using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Seguridad;

namespace Servicio.Hotel.Business.Interfaces.Seguridad
{
    public interface IRolService
    {
        Task<RolDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<RolDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<PagedResult<RolDTO>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<RolDTO> CreateAsync(RolCreateDTO rolCreateDto, CancellationToken ct = default);
        Task UpdateAsync(RolUpdateDTO rolUpdateDto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<RolDTO> GetByNombreAsync(string nombre, CancellationToken ct = default);
        Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default);
        Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null, CancellationToken ct = default);
    }
}