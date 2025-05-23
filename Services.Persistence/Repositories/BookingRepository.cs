using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ServiceDbContext _context;

        public BookingRepository(ServiceDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken) =>
            await _context
                .Set<Booking>()
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

        public async ValueTask<Booking> GetByIdAsync(
            Guid Id,
            CancellationToken cancellationToken
        ) =>
            await _context
                .Set<Booking>()
                .Include(c => c.Customer)
                .ThenInclude(p => p.Point)
                .AsTracking()
                .FirstAsync(id => id.Id == Id, cancellationToken);
    }
}
