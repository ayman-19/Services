using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly ServiceDbContext _context;

        public ServiceRepository(ServiceDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async ValueTask DeleteByIdAsync(Guid Id, CancellationToken cancellationToken) =>
            await _context
                .Set<Service>()
                .Where(s => s.Id == Id)
                .ExecuteDeleteAsync(cancellationToken);

        public async ValueTask<Service> GetByIdAsync(Guid Id) =>
            await _context.Set<Service>().Where(s => s.Id == Id).FirstAsync();
    }
}
