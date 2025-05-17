using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;
using Services.Domain.Entities;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Persistence.Data;
using Services.Shared.Enums;
using Services.Shared.ValidationMessages;

namespace Services.Persistence.Repositories
{
    public sealed class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ServiceDbContext _context;

        public UserRepository(ServiceDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<bool> EmailOrPhoneIsExist(LoginType type, string emailOrPhone)
        {
            if (type == LoginType.Email)
                return await _context.Set<User>().AnyAsync(email => email.Email == emailOrPhone);
            else if (type == LoginType.Phone)
                return await _context.Set<User>().AnyAsync(email => email.Phone == emailOrPhone);
            else
                return false;
        }

        public async Task<User> GetByIdAsync(Guid id) =>
            await _context.Set<User>().AsNoTracking().FirstAsync(user => user.Id == id);

        public async Task<User> GetByEmailAsync(string email) =>
            await _context
                .Set<User>()
                .AsTracking()
                .Include(user => user.Token)
                .Include(user => user.UserRoles)
                .Include(user => user.Branch)
                .FirstAsync(user => user.Email == email);

        public async Task<User> GetByEmailWithTrackinAsync(string email) =>
            await _context
                .Set<User>()
                .AsTracking()
                .Include(user => user.Token)
                .FirstAsync(user => user.Email == email);

        public async Task<User> GetByPhoneAsync(string phone) =>
            await _context
                .Set<User>()
                .AsTracking()
                .Include(user => user.Token)
                .FirstAsync(user => user.Phone == phone);

        public async Task UpdateCodeAsync(Guid Id, string code) =>
            await _context
                .Set<User>()
                .Where(u => u.Id == Id)
                .ExecuteUpdateAsync(p => p.SetProperty(p => p.Code, code));

        public async Task UpdateBranchAsync(Guid userId, double latitude, double longitude)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            var affectedRows = await _context
                .Set<Branch>()
                .Where(b => b.UserId == userId)
                .ExecuteUpdateAsync(b =>
                    b.SetProperty(branch => branch.Latitude, latitude)
                        .SetProperty(branch => branch.Langitude, longitude)
                );

            if (affectedRows == 0)
            {
                var newBranch = new Branch
                {
                    UserId = userId,
                    Latitude = latitude,
                    Langitude = longitude,
                };
                _context.Set<Branch>().Add(newBranch);
                await _context.SaveChangesAsync();
            }
        }
    }
}
