using Services.Domain.Abstraction;

namespace Services.Domain.Entities
{
    public sealed record CustomerBranch : ITrackableCreate
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public DateTime CreateOn { get; set; }

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;
    }
}
