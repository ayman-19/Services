using Services.Domain.Base;

namespace Services.Domain.Entities
{
    public sealed record DiscountRule : Entity<Guid>
    {
        public int MainPoints { get; set; }
        public Guid DiscountId { get; set; }
        public Discount? Discount { get; set; }
    }
}
