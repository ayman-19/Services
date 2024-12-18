using Microsoft.EntityFrameworkCore.Storage;

namespace Services.Domain.Abstraction
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default
        );
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
