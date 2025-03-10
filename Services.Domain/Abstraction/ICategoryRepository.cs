using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Services.Domain.Entities;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface ICategoryRepository : IRepository<Category>
    {
        ValueTask DeleteByIdAsync(Guid Id, CancellationToken cancellationToken);
        Task<Category> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
