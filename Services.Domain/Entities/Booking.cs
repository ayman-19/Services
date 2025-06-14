using Services.Domain.Abstraction;
using Services.Domain.Base;
using Services.Domain.Enums;

namespace Services.Domain.Entities
{
    public sealed record Booking : Entity<Guid>, ITrackableCreate
    {
        public string Description { get; set; }
        public DateTime CreateOn { get; set; }
        public BookingStatus Status { get; set; }
        public LocationType Location { get; set; }
        public double OldPrice { get; set; }
        public double UpdatedPrice { get; set; }
        public double? Rate { get; set; }
        public bool IsPaid { get; set; }
        public double UpdatedTax => UpdatedPrice * 0.14;
        public double UpdatedTotal => UpdatedPrice + UpdatedTax;
        public double OldTax => OldPrice * 0.14;
        public double OldTotal => OldPrice + OldTax;

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
            bool isPaid,
            double rate,
            BookingStatus status,
            string description
        )
        {
            CreateOn = createOn;
            Location = location;
            CustomerId = customerId;
            WorkerId = workerId;
            ServiceId = serviceId;
            Rate = rate;
            IsPaid = isPaid;
            Status = status;
            Description = description;
        }

        public void UpdateTax(double oldPrice, double updatedPrice)
        {
            OldPrice = oldPrice;
            UpdatedPrice = updatedPrice;
        }

        public void SetCreateOn()
        {
            CreateOn = DateTime.UtcNow;
            Status = BookingStatus.Pending;
        }
    }
}
