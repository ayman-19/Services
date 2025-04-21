using Services.Domain.Abstraction;
using Services.Domain.Base;
using Services.Domain.Enums;

namespace Services.Domain.Entities
{
    public sealed record Booking : Entity<Guid>, ITrackableCreate
    {
        public DateTime CreateOn { get; set; }
        public BookingStatus Status { get; set; }
        public LocationType Location { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid WorkerId { get; set; }
        public Worker Worker { get; set; }

        public void UpdateBooking(
            DateTime createOn,
            LocationType location,
            Guid customerId,
            Guid workerId
        )
        {
            CreateOn = createOn;
            Location = location;
            CustomerId = customerId;
            WorkerId = workerId;
        }

        public void SetCreateOn()
        {
            CreateOn = DateTime.UtcNow;
            Status = BookingStatus.Pending;
        }
    }
}
