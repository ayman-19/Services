using Services.Domain.Entities;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface IDiscountRuleRepository : IRepository<DiscountRule>
    {
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
        ValueTask<DiscountRule> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        ValueTask<(int, double)> GetPercentageOfPoint(
            int points,
            CancellationToken cancellationToken
        );
    }
}
