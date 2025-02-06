using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;
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
    }
}
