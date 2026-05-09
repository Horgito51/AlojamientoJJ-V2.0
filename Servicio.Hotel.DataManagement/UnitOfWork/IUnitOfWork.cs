using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Servicio.Hotel.DataManagement.UnitOfWork
{
    /// <summary>
    /// Unidad de trabajo para manejar transacciones y persistencia.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Guarda todos los cambios realizados en el contexto.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken ct = default);

        /// <summary>
        /// Inicia una transacción explícita.
        /// </summary>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);

        /// <summary>
        /// Obtiene la transacción actual (si existe).
        /// </summary>
        IDbContextTransaction? CurrentTransaction { get; }

        /// <summary>
        /// Ejecuta una acción dentro de una transacción.
        /// </summary>
        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken ct = default);
    }
}