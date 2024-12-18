using Services.Domain.Abstraction;
using Services.Domain.Base;

namespace Services.Domain.Models
{
    public sealed record Permission : Entity<Guid>, ITrackableCreate, ITrackableUpdate
    {
        private Permission(string name) => Name = name;

        public string Name { get; set; }
        public DateTime CreateOn {  get; set; } 
        public DateTime? UpdateOn { get; set; }
        public IReadOnlyCollection<RolePermission> RolePermissions { get; set; } =
            new List<RolePermission>();
        public static Permission Create(string name) => new(name);
        public void SetCreateOn() => CreateOn = DateTime.UtcNow;
        public void SetUpdateOn() => UpdateOn = DateTime.UtcNow;  
        
    }
}
