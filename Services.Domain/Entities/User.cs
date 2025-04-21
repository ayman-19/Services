using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Identity;
using Services.Domain.Abstraction;
using Services.Domain.Base;
using Services.Domain.Entities;
using Services.Domain.Enums;

namespace Services.Domain.Models
{
    public sealed record User : Entity<Guid>, ITrackableCreate, ITrackableDelete, ITrackableUpdate
    {
        public User() { }

        private User(string name, string email, string phone, UserType userType)
        {
            Name = name;
            Email = email;
            Phone = phone;
            UserType = userType;
            UserRoles = new HashSet<UserRole>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string? Code { get; set; }
        public string Phone { get; set; }
        public UserType UserType { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime DeleteOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public bool ConfirmAccount { get; set; }
        public string HashedPassword { get; set; }
        public Token Token { get; set; }
        public Worker Worker { get; set; }
        public Customer Customer { get; set; }
        public Branch Branch { get; set; }
        public IReadOnlyCollection<UserRole> UserRoles { get; set; }

        public void HashPassword(IPasswordHasher<User> passwordHasher, string password) =>
            HashedPassword = passwordHasher.HashPassword(this, password);

        public void HashedCode(IPasswordHasher<User> passwordHasher, string code) =>
            Code = passwordHasher.HashPassword(this, code);

        public static User Create(string name, string email, string phone, UserType userType) =>
            new(name, email, phone, userType);

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;

        public void SetDeleteOn() => DeleteOn = DateTime.UtcNow;

        public void SetUpdateOn() => UpdateOn = DateTime.UtcNow;

        public void Update(string name, string email, string phone)
        {
            this.Name = name;
            this.Email = email;
            this.Phone = phone;
            this.ConfirmAccount = false;
        }
    }
}
