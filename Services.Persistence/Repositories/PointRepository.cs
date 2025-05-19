using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class PointRepository : Repository<Point>, IPointRepository
    {
        private readonly ServiceDbContext context;

        public PointRepository(ServiceDbContext context)
            : base(context)
        {
            this.context = context;
        }

        public async Task UpdateForCustomerAsync(
            Guid customerId,
            CancellationToken cancellationToken
        )
        {
            var affectedRows = await context
                .Set<Point>()
                .Where(p => p.CustomerId == customerId)
                .ExecuteUpdateAsync(
                    p => p.SetProperty(c => c.Number, n => n.Number + 1),
                    cancellationToken
                );

            if (affectedRows == 0)
            {
                var newPoint = new Point { CustomerId = customerId, Number = 1 };

                await context.Set<Point>().AddAsync(newPoint, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
