using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Persistence.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ServiceDbContext context) : base(context)
        {
        }
    }
}
