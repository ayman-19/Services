using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace Services.Domain.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<TSelctor> GetAsync<TSelctor>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TSelctor>> Selctor,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>> includes = null!,
            bool astracking = true,
            CancellationToken cancellationToken = default
        );

        Task<IReadOnlyCollection<TSelctor>> GetAllAsync<TSelctor>(
            Expression<Func<TEntity, TSelctor>> Selctor,
            Expression<Func<TEntity, bool>> predicate = default!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null!,
            CancellationToken cancellationToken = default
        );
        ValueTask<EntityEntry<TEntity>> DeleteAsync(
            TEntity entity,
            CancellationToken cancellationToken = default
        );

        Task<bool> IsAnyExistAsync(
            Expression<Func<TEntity, bool>> pridecate,
            CancellationToken cancellationToken = default
        );
        Task<int> CountAsync(
            Expression<Func<TEntity, bool>> pridecate = null!,
            CancellationToken cancellationToken = default
        );
        ValueTask<EntityEntry<TEntity>> CreateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default
        );
        ValueTask<EntityEntry<TEntity>> UpdateAsync(
            TEntity entity,
            CancellationToken cancellationToken = default
        );
        Task<(IReadOnlyCollection<TSelctor>, int count)> PaginateAsync<TSelctor>(
            int page,
            int pageSize,
            Expression<Func<TEntity, TSelctor>> Selctor,
            Expression<Func<TEntity, bool>> predicate = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>> includes = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> ordered = null!,
            CancellationToken cancellationToken = default
        );
    }
}
