using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ServiceDbContext _context;

        public CategoryRepository(ServiceDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<Category> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
            await _context
                .Set<Category>()
                .Where(c => c.Id == id)
                .AsTracking()
                .FirstAsync(cancellationToken);

        public async ValueTask DeleteByIdAsync(Guid id, CancellationToken cancellationToken) =>
            await _context
                .Set<Category>()
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
    }
}
