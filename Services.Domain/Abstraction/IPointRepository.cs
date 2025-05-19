using Services.Domain.Entities;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface IPointRepository : IRepository<Point>
    {
        Task UpdateForCustomerAsync(Guid customerId, CancellationToken cancellationToken);
    }
}
