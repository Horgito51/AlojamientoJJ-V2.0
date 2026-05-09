using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.DTOs.Alojamiento;

namespace Servicio.Hotel.Business.Interfaces.Alojamiento
{
    public interface ICatalogoServicioService
    {
        Task<IEnumerable<CatalogoServicioDTO>> GetAllAsync(CancellationToken ct = default);
        Task<CatalogoServicioDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<CatalogoServicioDTO> CreateAsync(CatalogoServicioCreateDTO dto, CancellationToken ct = default);
        Task UpdateAsync(CatalogoServicioUpdateDTO dto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
