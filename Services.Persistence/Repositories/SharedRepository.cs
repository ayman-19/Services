using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Services.Domain.Repositories;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public class SharedRepository<TEntity> : ISharedRepository<TEntity>
        where TEntity : class
    {
        private readonly ServiceDbContext _context;
        private readonly DbSet<TEntity> _entities;

        public SharedRepository(ServiceDbContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }

        public Task<TSelctor> GetAsync<TSelctor>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TSelctor>> Selctor,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            bool astracking = true,
            CancellationToken cancellationToken = default
        )
        {
            var query = _entities.AsQueryable();

            if (!astracking)
                query = query.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            if (includes != null)
                query = includes.Invoke(query);

            return query.Select(Selctor).FirstAsync(cancellationToken);
        }

        public async Task<bool> IsAnyExistAsync(Expression<Func<TEntity, bool>> pridecate) =>
            await _entities.AnyAsync(pridecate);
    }
}
