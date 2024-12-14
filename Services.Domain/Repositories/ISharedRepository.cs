using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Services.Domain.Repositories
{
    public interface ISharedRepository<TEntity>
        where TEntity : class
    {
        Task<TSelctor> GetAsync<TSelctor>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TSelctor>> Selctor,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null!,
            bool astracking = true,
            CancellationToken cancellationToken = default
        );

        Task<IReadOnlyCollection<TSelctor>> GetAllAsync<TSelctor>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TSelctor>> Selctor,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null!,
            CancellationToken cancellationToken = default
        );
        Task DeleteAsync(TEntity entity);
        Task<bool> IsAnyExistAsync(
            Expression<Func<TEntity, bool>> pridecate,
            CancellationToken cancellationToken = default
        );
        Task<int> CountAsync(
            Expression<Func<TEntity, bool>> pridecate = null!,
            CancellationToken cancellationToken = default
        );
    }
}
