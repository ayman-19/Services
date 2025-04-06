using Services.Domain.Entities;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
       ValueTask<Discount> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
