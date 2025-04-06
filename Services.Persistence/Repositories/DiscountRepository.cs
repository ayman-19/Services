using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        private readonly ServiceDbContext _context;

        public DiscountRepository(ServiceDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken) =>
            await _context
                .Set<Discount>()
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

        public async ValueTask<Discount> GetByIdAsync(Guid Id, CancellationToken cancellationToken) =>
            await _context
                .Set<Discount>()
                .AsTracking()
                .FirstAsync(id => id.Id == Id, cancellationToken);

    
        }
    }

