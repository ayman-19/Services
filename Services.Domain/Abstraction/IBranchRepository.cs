using Services.Domain.Entities;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface IBranchRepository : IRepository<Branch>
    {
        ValueTask<Branch> GetByIdAsync(Guid Id, CancellationToken cancellationToken);
        ValueTask<Branch> GetByNameAsync(string Name, CancellationToken cancellationToken);
    }
}
