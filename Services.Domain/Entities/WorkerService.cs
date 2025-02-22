using Services.Domain.Abstraction;

namespace Services.Domain.Entities
{
    public sealed record WorkerService : ITrackableCreate, ITrackableUpdate
    {
        public Guid Id { get; set; }
        public Guid WorkerId { get; set; }
        public Guid BranchId { get; set; }
        public Guid ServiceId { get; set; }
        public bool Availabilty { get; set; }
        public Worker Worker { get; set; } = new();
        public Branch Branch { get; set; } = new();
        public Service Service { get; set; } = new();
        public DateTime CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;

        public void SetUpdateOn() => UpdateOn = DateTime.UtcNow;

        public void UpdateAvailabilty(bool availabilty) => Availabilty = availabilty;
    }
}
