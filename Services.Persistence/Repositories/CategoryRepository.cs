using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ServiceDbContext context)
            : base(context) { }
    }
}
