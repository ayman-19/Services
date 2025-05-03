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
        public double Price { get; set; }
        public int? Rate { get; set; }
        public double Tax => Price * 0.14;
        public double Total => Price + Tax;

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid WorkerId { get; set; }
        public Worker Worker { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }

        public void UpdateBooking(
            DateTime createOn,
            LocationType location,
            Guid customerId,
            Guid workerId,
            Guid serviceId,
            double total,
            int rate
        )
        {
            CreateOn = createOn;
            Location = location;
            CustomerId = customerId;
            WorkerId = workerId;
            Price = total;
            ServiceId = serviceId;
            Rate = rate;
        }

        public void SetCreateOn()
        {
            CreateOn = DateTime.UtcNow;
            Status = BookingStatus.Pending;
        }
    }
}
