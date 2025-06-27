using Services.Domain.Abstraction;
using Services.Domain.Base;
using Services.Domain.Enums;

namespace Services.Domain.Entities
{
    public sealed record Booking : Entity<Guid>, ITrackableCreate
    {
        public string Description { get; set; } = string.Empty;
        public DateTime CreateOn { get; set; }
        public BookingStatus Status { get; set; }
        public LocationType Location { get; set; }

        public double OldPrice { get; set; }
        public double UpdatedPrice { get; private set; }
        public double? Rate { get; set; }
        public bool IsPaid { get; set; }

        public double UpdatedTax => CalculateTax(UpdatedPrice);
        public double UpdatedTotal => CalculateTotal(UpdatedPrice);

        public double OldTax => CalculateTax(OldPrice);
        public double OldTotal => CalculateTotal(OldPrice);

        public Guid CustomerId { get; set; }
        public Customer Customer { get; private set; }

        public Guid WorkerId { get; set; }
        public Worker Worker { get; private set; }

        public Guid ServiceId { get; set; }
        public Service Service { get; private set; }

        public void UpdatePricing(double oldPrice, double updatedPrice)
        {
            if (oldPrice < 0 || updatedPrice < 0)
                throw new ArgumentOutOfRangeException("Prices cannot be negative.");

            OldPrice = oldPrice;
            UpdatedPrice = updatedPrice;
        }

        public void UpdateBookingDetails(
            DateTime createOn,
            LocationType location,
            Guid customerId,
            Guid workerId,
            Guid serviceId,
            bool isPaid,
            double? rate,
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

        private static double CalculateTax(double price) => price * 0.14;

        private double CalculateTotal(double price) => IsPaid ? price + CalculateTax(price) : price;

        public void SetCreateOn()
        {
            CreateOn = DateTime.UtcNow;
            Status = BookingStatus.Pending;
        }
    }
}
