using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
        ValueTask<Booking> GetByIdAsync(Guid Id, CancellationToken cancellationToken);
        Task DeleteByUserIdAsync(Guid id, UserType type, CancellationToken cancellationToken);
        Task<double> GetTotalPriceAsync(
            Guid id,
            BookingStatus status,
            CancellationToken cancellationToken
        );
    }
}
