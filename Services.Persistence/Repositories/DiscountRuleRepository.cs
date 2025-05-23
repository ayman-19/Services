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
            : base(context) { }

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

        public async ValueTask<(int, double)> GetPercentageOfPoint(
            int points,
            CancellationToken cancellationToken
        )
        {
            var applicableRule = await _context
                .Set<DiscountRule>()
                .AsNoTracking()
                .Where(rule => rule.MainPoints <= points)
                .OrderByDescending(rule => rule.MainPoints)
                .Select(rule => new { rule.MainPoints, rule.Discount.Percentage })
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            return applicableRule == null
                ? (Points: 0, Percentage: 0)
                : (Points: applicableRule.MainPoints, Percentage: applicableRule.Percentage);
        }
    }
}
