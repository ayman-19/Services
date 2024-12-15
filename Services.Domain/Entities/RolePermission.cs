using System.ComponentModel.DataAnnotations.Schema;
using Services.Domain.Base;

namespace Services.Domain.Models
{
    public sealed record RolePermission : Entity<Guid>
    {
        private RolePermission(Guid roleId, Guid permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }

        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public DateTime CreateOn => DateTime.UtcNow;
        public DateTime? UpdateOn { get; set; }
        public Role? Role { get; set; }
        public Permission? Permission { get; set; }

        public static RolePermission Create(Guid roleId, Guid permissionId) =>
            new RolePermission(roleId, permissionId);
    }
}
