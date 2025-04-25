using Services.Domain.Models;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface IPermissionRepository : IRepository<Permission> { }
}
