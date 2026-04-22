using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;

namespace ChainDegree.Application.Common.Persistence
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        // Check if transaction is active
        bool HasActiveTransaction { get; }

        // Simple scenarios (auto-transaction)
        Task<int> CommitAsync(CancellationToken ct = default);

        // Complex scenarios (explicit transaction)
        Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync(CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
