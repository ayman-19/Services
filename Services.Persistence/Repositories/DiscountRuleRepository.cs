using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class DiscountRuleRepository : Repository<DiscountRule>, IDiscountRuleRepository
    {
        public DiscountRuleRepository(ServiceDbContext context)
            : base(context) { }
    }
}
