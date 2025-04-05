using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        public DiscountRepository(ServiceDbContext context)
            : base(context) { }
    }
}
