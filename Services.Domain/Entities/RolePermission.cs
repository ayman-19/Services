using System.ComponentModel.DataAnnotations.Schema;
using Services.Domain.Abstraction;
using Services.Domain.Base;

namespace Services.Domain.Models
{

    public sealed record RolePermission : Entity<Guid>, ITrackableCreate, ITrackableUpdate
    {
        private RolePermission(Guid roleId, Guid permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }

        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public DateTime CreateOn { get; set; }  
        public DateTime? UpdateOn { get; set; }
        public Role? Role { get; set; }
        public Permission? Permission { get; set; }
        DateTime ITrackableCreate.CreateOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public static RolePermission Create(Guid roleId, Guid permissionId) =>
            new RolePermission(roleId, permissionId);
        public void SetCreateOn() => CreateOn = DateTime.UtcNow;
        public void SetUpdateOn() => UpdateOn = DateTime.UtcNow;    
      
    }
}
