using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(ServiceDbContext context) : base(context)
        {
        }
    }
}
