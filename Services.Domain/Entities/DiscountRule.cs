using Services.Domain.Abstraction;
using Services.Domain.Base;

namespace Services.Domain.Entities
{
    public sealed record DiscountRule : Entity<Guid>, ITrackableCreate
    {
        public int MainPoints { get; set; }
        public Guid DiscountId { get; set; }
        public Discount? Discount { get; set; }
        public DateTime CreateOn { get; set; }

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;
    }
}
