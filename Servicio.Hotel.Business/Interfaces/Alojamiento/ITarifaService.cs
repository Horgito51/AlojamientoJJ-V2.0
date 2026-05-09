using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Alojamiento;

namespace Servicio.Hotel.Business.Interfaces.Alojamiento
{
    public interface ITarifaService
    {
        Task<TarifaDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<TarifaDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<TarifaDTO>> GetAllAsync(CancellationToken ct = default);
        Task<TarifaDTO> CreateAsync(TarifaCreateDTO tarifaCreateDto, CancellationToken ct = default);
        Task UpdateAsync(TarifaUpdateDTO tarifaUpdateDto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<IEnumerable<TarifaDTO>> GetBySucursalAsync(int idSucursal, CancellationToken ct = default);
        Task<TarifaDTO> GetTarifaVigenteAsync(int idSucursal, int idTipoHabitacion, DateTime fecha, string? canal = null, CancellationToken ct = default);
        Task<TarifaDTO?> GetTarifaVigenteRangoOrDefaultAsync(int idSucursal, int idTipoHabitacion, DateTime fechaInicio, DateTime fechaFin, string? canal = null, CancellationToken ct = default);
        Task DesactivarAsync(int id, string usuario, CancellationToken ct = default);
    }
}
