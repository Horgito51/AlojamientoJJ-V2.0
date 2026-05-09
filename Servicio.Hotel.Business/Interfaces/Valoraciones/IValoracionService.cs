using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Valoraciones;

namespace Servicio.Hotel.Business.Interfaces.Valoraciones
{
    public interface IValoracionService
    {
        Task<ValoracionDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ValoracionDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<PagedResult<ValoracionDTO>> GetByFiltroAsync(ValoracionFiltroDTO filtro, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<ValoracionDTO> CreateAsync(ValoracionDTO valoracionDto, CancellationToken ct = default);
        Task UpdateAsync(ValoracionDTO valoracionDto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task ModerarAsync(int idValoracion, string nuevoEstado, string motivo, string moderador, CancellationToken ct = default);
        Task ResponderAsync(int idValoracion, string respuesta, string usuario, CancellationToken ct = default);
        Task<bool> ExistsByEstadiaAsync(int idEstadia, CancellationToken ct = default);
    }
}