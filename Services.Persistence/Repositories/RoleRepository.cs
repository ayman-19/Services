using Microsoft.EntityFrameworkCore;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly ServiceDbContext _context;

        public RoleRepository(ServiceDbContext context)
            : base(context) => _context = context;

        public async Task<Guid> GetRoleIdByNameAsync(string Name) =>
            await _context.Set<Role>().Where(r => r.Name == Name).Select(r => r.Id).FirstAsync();
    }
}
