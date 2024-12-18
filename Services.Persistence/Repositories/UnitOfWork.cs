using Microsoft.EntityFrameworkCore.Storage;
using Services.Domain.Abstraction;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ServiceDbContext _context;

        public UnitOfWork(ServiceDbContext context) => _context = context;

        public async Task<IDbContextTransaction> BeginTransactionAsync(
            CancellationToken cancellationToken = default
        ) => await _context.Database.BeginTransactionAsync(cancellationToken);

        public async ValueTask DisposeAsync() => await _context.DisposeAsync();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _context.SaveChangesAsync(cancellationToken);
    }
}
