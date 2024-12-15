using Microsoft.AspNetCore.Identity;
using Services.Domain.Base;
using Services.Domain.Entities;

namespace Services.Domain.Models
{
    public sealed record Role : Entity<Guid>
    {
        private Role(string name) => Name = name;

        public string Name { get; set; }
        public DateTime CreateOn => DateTime.UtcNow;
        public DateTime? UpdateOn { get; set; }

        public static Role Create(string name) => new(name);

        public IReadOnlyCollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public IReadOnlyCollection<RolePermission> RolePermissions { get; set; } =
            new List<RolePermission>();
    }
}
