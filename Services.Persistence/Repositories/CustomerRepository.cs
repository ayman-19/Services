using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ServiceDbContext context)
            : base(context) { }
    }
}
