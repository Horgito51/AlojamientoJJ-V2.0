using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Servicio.Hotel.DataAccess.Entities.Seguridad;

namespace Servicio.Hotel.DataAccess.Repositories.Interfaces.Seguridad
{
    public interface IUsuarioRolRepository
    {
        // CRUD básico
        Task<UsuarioRolEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<UsuarioRolEntity>> GetAllAsync(CancellationToken ct = default);
        Task<UsuarioRolEntity> AddAsync(UsuarioRolEntity entity, CancellationToken ct = default);

        // Operaciones de escritura
        Task DeleteByUsuarioAndRolAsync(int idUsuario, int idRol, CancellationToken ct = default);
        Task DeleteAllByUsuarioAsync(int idUsuario, CancellationToken ct = default);
        Task<bool> ExistsAsync(int idUsuario, int idRol, CancellationToken ct = default);
        Task<IEnumerable<int>> GetRolesIdsByUsuarioAsync(int idUsuario, CancellationToken ct = default);
    }
}