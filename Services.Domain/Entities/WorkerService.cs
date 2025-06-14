using Services.Domain.Abstraction;

namespace Services.Domain.Entities
{
    public sealed record WorkerService : ITrackableCreate, ITrackableUpdate
    {
        public Guid Id { get; set; }
        public Guid WorkerId { get; set; }
        public Guid ServiceId { get; set; }
        public bool Availabilty { get; set; }
        public double Price { get; set; }
        public double? Discount { get; set; }
        public double Rate { get; set; }
        public Worker Worker { get; set; }
        public Service Service { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;

        public void SetUpdateOn() => UpdateOn = DateTime.UtcNow;

        public void UpdateAvailabilty(bool availabilty) => Availabilty = availabilty;
    }
}
