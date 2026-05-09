using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.DTOs.Alojamiento;

namespace Servicio.Hotel.Business.Interfaces.Alojamiento
{
    public interface ITipoHabitacionService
    {
        Task<TipoHabitacionDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<TipoHabitacionDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<TipoHabitacionDTO> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<IEnumerable<TipoHabitacionDTO>> GetAllAsync(CancellationToken ct = default);
        Task<TipoHabitacionDTO> CreateAsync(TipoHabitacionCreateDTO tipoCreateDto, CancellationToken ct = default);
        Task UpdateAsync(TipoHabitacionUpdateDTO tipoUpdateDto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<IEnumerable<TipoHabitacionDTO>> GetPublicosAsync(CancellationToken ct = default);
        Task<bool> ExistsByCodigoAsync(string codigo, CancellationToken ct = default);
    }
}
