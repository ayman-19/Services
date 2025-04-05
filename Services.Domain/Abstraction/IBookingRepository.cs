using Services.Domain.Entities;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
