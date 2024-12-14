namespace Services.Domain.Models
{
    public class Permission
    {
        public IList<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
