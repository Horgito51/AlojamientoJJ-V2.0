using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Servicio.Hotel.DataAccess.Context;

namespace Servicio.Hotel.DataManagement.UnitOfWork
{
    /// <summary>
    /// Implementación de UnitOfWork usando el DbContext de Entity Framework.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ServicioHotelDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(ServicioHotelDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IDbContextTransaction? CurrentTransaction => _currentTransaction;

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("Ya existe una transacción activa.");
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync(ct);
            return _currentTransaction;
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken ct = default)
        {
            if (_currentTransaction != null)
            {
                // Ya hay una transacción, simplemente ejecutar la acción
                await action();
                return;
            }

            await using var transaction = await BeginTransactionAsync(ct);
            try
            {
                await action();
                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
            finally
            {
                _currentTransaction = null;
            }
        }

        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }
}