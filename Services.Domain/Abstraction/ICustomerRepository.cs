using Services.Domain.Entities;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface ICustomerRepository : IRepository<Customer> { }
}
