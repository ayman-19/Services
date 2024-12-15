using Microsoft.AspNetCore.Identity;
using Services.Domain.Entities;

namespace Services.Domain.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public Token Token { get; set; }

        public void HashPassword(IPasswordHasher<User> passwordHasher, string password) =>
            passwordHasher.HashPassword(this, password);
    }
}
