using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Persistence.Repositories
{
    public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(ServiceDbContext context) : base(context)
        {
        }
    }
}
