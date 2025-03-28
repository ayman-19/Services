using Services.Domain.Abstraction;
using Services.Domain.Base;

namespace Services.Domain.Entities
{
    public sealed record Discound : Entity<Guid>, ITrackableCreate
    {
        public double Percentage { get; set; }
        public DateTime ExpireOn { get; set; }
        public DateTime CreateOn { get; set; }

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;
    }
}
