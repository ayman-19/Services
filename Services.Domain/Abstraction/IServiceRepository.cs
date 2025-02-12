using Services.Domain.Entities;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface IServiceRepository : IRepository<Service>
    {
        ValueTask<Service> GetByIdAsync(Guid Id);
        ValueTask DeleteByIdAsync(Guid Id, CancellationToken cancellationToken);
    }
}
