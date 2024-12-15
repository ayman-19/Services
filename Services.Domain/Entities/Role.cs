using Microsoft.AspNetCore.Identity;

namespace Services.Domain.Models
{
    public class Role : IdentityRole
    {
        public IList<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
