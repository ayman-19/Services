using Services.Domain.Base;
using Services.Domain.Models;

namespace Services.Domain.Entities
{
    public sealed record UserRole : Entity<Guid>
    {
        private UserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime CreatedOn => DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }

        public static UserRole Create(Guid userId, Guid roleId) => new UserRole(userId, roleId);
    }
}
