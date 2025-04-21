using Services.Domain.Models;
using Services.Shared.Enums;

namespace Services.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User> GetByIdAsync(Guid id);
        public Task<bool> EmailOrPhoneIsExist(LoginType type, string emailOrPhone);
        public Task UpdateCodeAsync(Guid Id, string code);

        public Task<User> GetByEmailAsync(string email);
        public Task<User> GetByPhoneAsync(string phone);

        public Task UpdateBranchAsync(Guid userId, double Latitude, double Longitude);
    }
}
