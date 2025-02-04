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

        public async Task<bool> EmailOrPhoneIsExist(string emailOrPhone)
        {
            if (new EmailAddressAttribute().IsValid(emailOrPhone))
                return await _context.Set<User>().AnyAsync(email => email.Email == emailOrPhone);
            else
            {
                bool validPhoneNumber = false;
                try
                {
                    PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
                    PhoneNumber number = phoneNumberUtil.Parse(emailOrPhone, "eg");
                    validPhoneNumber = phoneNumberUtil.IsValidNumber(number);
                }
                catch
                {
                    throw new ValidationException(ValidationMessages.User.ValidatePhoneNumber);
                }
                return validPhoneNumber
                    ? await _context.Set<User>().AnyAsync(email => email.Phone == emailOrPhone)
                    : false;
            }
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
            await _context.Set<User>().AsTracking().FirstAsync(user => user.Phone == phone);
    }
}
