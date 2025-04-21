using Services.Domain.Abstraction;
using Services.Domain.Models;

namespace Services.Domain.Entities
{
    public sealed record Branch : ITrackableCreate, ITrackableUpdate
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Langitude { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }

        public void UpdaeBranch(double latitude, double langitude)
        {
            Latitude = latitude;
            Langitude = langitude;
        }

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;

        public void SetUpdateOn() => UpdateOn = DateTime.UtcNow;
    }
}
