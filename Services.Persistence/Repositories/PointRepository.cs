using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class PointRepository : Repository<Point>, IPointRepository
    {
        public PointRepository(ServiceDbContext context)
            : base(context) { }
    }
}
