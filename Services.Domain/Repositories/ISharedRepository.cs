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
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            bool astracking = true,
            CancellationToken cancellationToken = default
        );
        Task<bool> IsAnyExistAsync(Expression<Func<TEntity, bool>> pridecate);
    }
}
