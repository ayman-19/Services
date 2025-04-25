using Services.Domain.Models;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface IRolePermissionRepository : IRepository<RolePermission>
    {
        Task DeleteTokenForUsersAssignThisRole(Guid RoleId, CancellationToken cancellationToken);
    }
}
