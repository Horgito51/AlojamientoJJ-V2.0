using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.DTOs.Alojamiento;

namespace Servicio.Hotel.Business.Interfaces.Alojamiento
{
    public interface ISucursalService
    {
        Task<IEnumerable<SucursalDTO>> GetAllAsync(string? estado = null, CancellationToken ct = default);
        Task<SucursalDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<SucursalDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<SucursalDTO> CreateAsync(SucursalCreateDTO dto, CancellationToken ct = default);
        Task UpdateAsync(SucursalUpdateDTO dto, CancellationToken ct = default);
        Task UpdatePoliticasAsync(Guid sucursalGuid, SucursalPoliticasUpdateDTO dto, string usuario, CancellationToken ct = default);
        Task InhabilitarAsync(Guid sucursalGuid, string motivo, string usuario, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task DeleteAsync(Guid guid, CancellationToken ct = default);
    }
}
