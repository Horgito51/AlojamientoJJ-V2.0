using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Alojamiento;

namespace Servicio.Hotel.Business.Interfaces.Alojamiento
{
    public interface IHabitacionService
    {
        Task<HabitacionDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<HabitacionDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<HabitacionDTO>> GetAllAsync(CancellationToken ct = default);
        Task<HabitacionDTO> CreateAsync(HabitacionCreateDTO habitacionCreateDto, CancellationToken ct = default);
        Task UpdateAsync(HabitacionUpdateDTO habitacionUpdateDto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<IEnumerable<HabitacionDTO>> GetBySucursalAsync(int idSucursal, CancellationToken ct = default);
        Task<IEnumerable<HabitacionDTO>> GetByTipoHabitacionAsync(int idTipoHabitacion, CancellationToken ct = default);
        Task UpdateEstadoAsync(int id, string nuevoEstado, string usuario, CancellationToken ct = default);
        Task<IEnumerable<HabitacionDTO>> GetDisponiblesAsync(int idSucursal, DateTime inicio, DateTime fin, CancellationToken ct = default);
    }
}