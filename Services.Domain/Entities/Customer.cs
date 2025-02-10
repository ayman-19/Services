using Services.Domain.Models;

namespace Services.Domain.Entities
{
    public sealed record Customer
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<CustomerBranch> CustomerBranchs { get; set; }

        public Customer() => CustomerBranchs = new List<CustomerBranch>();
    }
}
