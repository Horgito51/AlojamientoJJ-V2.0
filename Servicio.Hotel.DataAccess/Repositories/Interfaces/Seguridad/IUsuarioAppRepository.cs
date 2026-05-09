using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Seguridad;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Seguridad
{
    public interface IUsuarioAppRepository
    {
        // CRUD básico
        Task<UsuarioAppEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<UsuarioAppEntity?> GetByGuidAsync(Guid guid, CancellationToken ct = default);
        Task<IEnumerable<UsuarioAppEntity>> GetAllAsync(CancellationToken ct = default);
        Task<UsuarioAppEntity> AddAsync(UsuarioAppEntity entity, CancellationToken ct = default);
        Task UpdateAsync(UsuarioAppEntity entity, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        // Operaciones de escritura
        Task InhabilitarAsync(int id, string motivo, string usuario, CancellationToken ct = default);
        Task CambiarPasswordAsync(int id, string newHash, string newSalt, string usuario, CancellationToken ct = default);

        // Consultas para autenticación
        Task<UsuarioAppEntity?> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task<UsuarioAppEntity?> GetByCorreoAsync(string correo, CancellationToken ct = default);
        Task<UsuarioAppEntity?> AuthenticateAsync(string username, string passwordHash, CancellationToken ct = default);

        // Validaciones
        Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null, CancellationToken ct = default);
        Task<bool> ExistsByCorreoAsync(string correo, int? excludeId = null, CancellationToken ct = default);
    }
}