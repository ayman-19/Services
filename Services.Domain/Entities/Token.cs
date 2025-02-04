using Services.Domain.Abstraction;
using Services.Domain.Base;
using Services.Domain.Models;

namespace Services.Domain.Entities
{
    public sealed record Token : Entity<Guid>, ITrackableCreate, ITrackableUpdate
    {
        private Token(string content, DateTime expireOn, Guid userId)
        {
            Content = content;
            ExpireOn = expireOn;
            UserId = userId;
        }

        public string Content { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public DateTime ExpireOn { get; set; }
        public bool IsExpire => ExpireOn <= DateTime.Now;
        public bool IsValid => !IsExpire;
        public Guid UserId { get; set; }
        public User User { get; set; }

        public static Token Create(string content, DateTime expireOn, Guid userId) =>
            new(content, expireOn, userId);

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;

        public void SetUpdateOn() => UpdateOn = DateTime.UtcNow;
    }
}
