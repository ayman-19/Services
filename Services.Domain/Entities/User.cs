using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Identity;
using Services.Domain.Base;
using Services.Domain.Entities;

namespace Services.Domain.Models
{
    public sealed record User : Entity<Guid>
    {
        private User(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public DateTime CreateOn => DateTime.UtcNow;
        public DateTime? UpdateOn { get; set; }
        public Token Token { get; set; }

        public void HashPassword(IPasswordHasher<User> passwordHasher, string password) =>
            passwordHasher.HashPassword(this, password);

        public static User Create(string name, string userName, string email) =>
            new User(name, email);
    }
}
