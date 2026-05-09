using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.DataManagement.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio.Hotel.Business.Interfaces.Reservas
{
    public interface IClienteService
    {
        Task<ClienteDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ClienteDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<PagedResult<ClienteDTO>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<ClienteDTO> CreateAsync(ClienteCreateDTO clienteCreateDto, CancellationToken ct = default);
        Task UpdateAsync(ClienteUpdateDTO clienteUpdateDto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<ClienteDTO> GetByIdentificacionAsync(string tipo, string numero, CancellationToken ct = default);
        Task<ClienteDTO> GetByCorreoAsync(string correo, CancellationToken ct = default);
        Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default);
    }
}