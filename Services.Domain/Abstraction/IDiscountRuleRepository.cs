using System.Linq.Expressions;
using Services.Domain.Entities;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface IDiscountRuleRepository : IRepository<DiscountRule>
    {
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
        ValueTask<DiscountRule> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        ValueTask<(int Points, double Percentage)> GetPercentageOfPoint(
            int points,
            CancellationToken cancellationToken
        );
        ValueTask<TResult> GetNearestPointsAsync<TResult>(
            int points,
            Expression<Func<DiscountRule, TResult>> Selctor,
            CancellationToken cancellationToken
        );
    }
}
