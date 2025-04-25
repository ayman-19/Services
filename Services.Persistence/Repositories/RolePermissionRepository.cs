using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Domain.Models;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
    {
        private readonly ServiceDbContext _context;

        public RolePermissionRepository(ServiceDbContext context)
            : base(context) => _context = context;

        public async Task DeleteTokenForUsersAssignThisRole(
            Guid RoleId,
            CancellationToken cancellationToken
        )
        {
            await _context
                .Set<Token>()
                .Where(t => t.User.UserRoles.Any(r => r.RoleId == RoleId))
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
