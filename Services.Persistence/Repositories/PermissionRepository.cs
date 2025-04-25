using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    internal class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ServiceDbContext context)
            : base(context) { }
    }
}
