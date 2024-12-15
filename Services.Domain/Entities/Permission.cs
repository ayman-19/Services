using Services.Domain.Base;

namespace Services.Domain.Models
{
    public sealed record Permission : Entity<Guid>
    {
        private Permission(string name) => Name = name;

        public string Name { get; set; }
        public DateTime CreateOn => DateTime.UtcNow;
        public DateTime? UpdateOn { get; set; }
        public IReadOnlyCollection<RolePermission> RolePermissions { get; set; } =
            new List<RolePermission>();

        public static Permission Create(string name) => new(name);
    }
}
