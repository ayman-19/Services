using Services.Domain.Abstraction;
using Services.Domain.Models;

namespace Services.Domain.Entities
{
    public sealed record Token: ITrackableCreate, ITrackableUpdate
    {
        private Token(string content) => Content = content;
        public string Content { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public static Token Create(string content) => new(content);
        public void SetCreateOn() => CreateOn = DateTime.UtcNow;
        public void SetUpdateOn() => UpdateOn = DateTime.UtcNow; 
    }
}
