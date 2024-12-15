using Services.Domain.Models;

namespace Services.Domain.Entities
{
    public sealed class Token
    {
        public string Content { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
