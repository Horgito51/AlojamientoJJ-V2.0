using System;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataManagement.Seguridad.Models;
using Servicio.Hotel.DataManagement.Common;

namespace Servicio.Hotel.DataManagement.Seguridad.Interfaces
{
    public interface IUsuarioDataService
    {
        Task<UsuarioDataModel?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<UsuarioDataModel?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<DataPagedResult<UsuarioDataModel>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
        Task<UsuarioDataModel> AddAsync(UsuarioDataModel model, CancellationToken ct = default);
        Task UpdateAsync(UsuarioDataModel model, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<UsuarioDataModel?> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task<UsuarioCredencialesDataModel?> GetCredentialsByUsernameAsync(string username, CancellationToken ct = default);
        Task<UsuarioCredencialesDataModel?> GetCredentialsByCorreoAsync(string correo, CancellationToken ct = default);
        Task<UsuarioCredencialesDataModel?> GetCredentialsByIdAsync(int id, CancellationToken ct = default);
        Task<UsuarioDataModel?> GetByCorreoAsync(string correo, CancellationToken ct = default);
        Task<UsuarioDataModel?> AuthenticateAsync(string username, string passwordHash, CancellationToken ct = default);
        Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default);
        Task CambiarPasswordAsync(int id, string newHash, string newSalt, string usuario, CancellationToken ct = default);
        Task AsociarClienteAsync(int idUsuario, int idCliente, string usuario, CancellationToken ct = default);
        Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null, CancellationToken ct = default);
        Task<bool> ExistsByCorreoAsync(string correo, int? excludeId = null, CancellationToken ct = default);
    }
}
