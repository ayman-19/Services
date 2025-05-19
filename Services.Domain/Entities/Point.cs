using Services.Domain.Abstraction;
using Services.Domain.Base;

namespace Services.Domain.Entities
{
    public sealed record Point : Entity<Guid>, ITrackableCreate
    {
        public int Number { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime CreateOn { get; set; }

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;
    }
}
