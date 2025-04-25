using Services.Domain.Models;

namespace Services.Domain.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Guid> GetRoleIdByNameAsync(string Name);
    }
}
