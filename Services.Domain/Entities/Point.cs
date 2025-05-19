using Services.Domain.Base;

namespace Services.Domain.Entities
{
    public sealed record Point : Entity<Guid>
    {
        public int Number { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
