using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class DiscountRuleRepository : Repository<DiscountRule>, IDiscountRuleRepository
    {
        private readonly ServiceDbContext _context;

        public DiscountRuleRepository(ServiceDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken) =>
            await _context
                .Set<DiscountRule>()
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

        public async ValueTask<DiscountRule> GetByIdAsync(
            Guid Id,
            CancellationToken cancellationToken
        ) =>
            await _context
                .Set<DiscountRule>()
                .AsTracking()
                .FirstAsync(id => id.Id == Id, cancellationToken);

        public async ValueTask<TResult> GetNearestPointsAsync<TResult>(
            int points,
            Expression<Func<DiscountRule, TResult>> Selctor,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var higherOrEqualRule = await _context
                    .Set<DiscountRule>()
                    .AsNoTracking()
                    .Where(rule => rule.MainPoints >= points)
                    .OrderBy(rule => rule.MainPoints)
                    .Include(rule => rule.Discount)
                    .Select(Selctor)
                    .FirstOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (higherOrEqualRule != null)
                {
                    return higherOrEqualRule;
                }

                var smallerRule = await _context
                    .Set<DiscountRule>()
                    .AsNoTracking()
                    .Where(rule => rule.MainPoints < points)
                    .OrderByDescending(rule => rule.MainPoints)
                    .Include(rule => rule.Discount)
                    .Select(Selctor)
                    .FirstOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);

                return smallerRule ?? default!;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async ValueTask<(int Points, double Percentage)> GetPercentageOfPoint(
            int points,
            CancellationToken cancellationToken
        )
        {
            var applicableRule = await _context
                .Set<DiscountRule>()
                .AsNoTracking()
                .Include(d => d.Discount)
                .Where(rule => rule.MainPoints <= points && rule.Discount != null)
                .OrderByDescending(rule => rule.MainPoints)
                .FirstOrDefaultAsync(cancellationToken);

            if (applicableRule is null)
                return (Points: 0, Percentage: 0);

            return (
                Points: applicableRule.MainPoints,
                Percentage: applicableRule.Discount.Percentage
            );
        }
    }
}
