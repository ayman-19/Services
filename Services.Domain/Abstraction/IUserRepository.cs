using Services.Domain.Models;

namespace Services.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> GetByIdAsync(Guid id);
        public Task<bool> EmailOrPhoneIsExist(string emailOrPhone);

        public Task<User> GetByEmailAsync(string email);
        public Task<User> GetByPhoneAsync(string phone);
    }
}
