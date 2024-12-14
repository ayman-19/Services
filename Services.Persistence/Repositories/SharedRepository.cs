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

        public async Task<int> CountAsync(
            Expression<Func<TEntity, bool>> pridecate = null!,
            CancellationToken cancellationToken = default
        )
        {
            if (pridecate == null)
                return await _entities.CountAsync(cancellationToken);

            return await _entities.CountAsync(pridecate, cancellationToken);
        }

        public Task DeleteAsync(TEntity entity) => Task.Run(() => _entities.Remove(entity));

        public async Task<IReadOnlyCollection<TSelctor>> GetAllAsync<TSelctor>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TSelctor>> Selctor,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null!,
            CancellationToken cancellationToken = default
        )
        {
            var query = _entities.AsQueryable().AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            if (includes != null)
                query = includes.Invoke(query);

            return await query.Select(Selctor).ToListAsync(cancellationToken);
        }

        public Task<TSelctor> GetAsync<TSelctor>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TSelctor>> Selctor,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null!,
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

        public async Task<bool> IsAnyExistAsync(
            Expression<Func<TEntity, bool>> pridecate,
            CancellationToken cancellationToken = default
        ) => await _entities.AnyAsync(pridecate, cancellationToken);
    }
}
