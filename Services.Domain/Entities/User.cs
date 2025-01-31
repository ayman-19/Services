using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Identity;
using Services.Domain.Abstraction;
using Services.Domain.Base;
using Services.Domain.Entities;

namespace Services.Domain.Models
{
    public sealed record User : Entity<Guid>, ITrackableCreate, ITrackableDelete, ITrackableUpdate
    {
        private User(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string? Code { get; set; }
		public string Phone { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime DeleteOn { get; set; }
        public DateTime? UpdateOn { get; set; }
		public bool ConfirmAccount { get; set; }
		public string HashedPassword { get; set; }
        public Token Token { get; set; }
        public IReadOnlyCollection<UserRole> UserRoles { get; set; }

        public void HashPassword(IPasswordHasher<User> passwordHasher, string password) =>
            HashedPassword = passwordHasher.HashPassword(this, password);

        public void HashedCode(IPasswordHasher<User> passwordHasher, string code) =>
            Code = passwordHasher.HashPassword(this, code);

        public static User Create(string name, string email, string phone) => new(name, email, phone);

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;

        public void SetDeleteOn() => DeleteOn = DateTime.UtcNow;

        public void SetUpdateOn() => UpdateOn = DateTime.UtcNow;
    }
}
