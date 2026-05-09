using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.Business.Common;
using Servicio.Hotel.Business.DTOs.Seguridad;

namespace Servicio.Hotel.Business.Interfaces.Seguridad
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO> GetByIdAsync(int id, CancellationToken ct = default);
        Task<UsuarioDTO> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<PagedResult<UsuarioDTO>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<UsuarioDTO> CreateAsync(UsuarioCreateDTO usuarioCreateDto, CancellationToken ct = default);
        Task UpdateAsync(UsuarioUpdateDTO usuarioUpdateDto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<UsuarioDTO> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task<UsuarioDTO> GetByCorreoAsync(string correo, CancellationToken ct = default);
        Task AsociarClienteAsync(int idUsuario, int idCliente, string usuario, CancellationToken ct = default);
        Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default);
        Task CambiarPasswordAsync(int id, string newPassword, string usuario, CancellationToken ct = default);
        Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null, CancellationToken ct = default);
        Task<bool> ExistsByCorreoAsync(string correo, int? excludeId = null, CancellationToken ct = default);
    }
}
