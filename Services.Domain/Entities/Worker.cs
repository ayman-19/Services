using Services.Domain.Enums;
using Services.Domain.Models;

namespace Services.Domain.Entities
{
    public sealed record Worker
    {
        public double Experience { get; set; }
        public Status Status { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = new();
        public ICollection<WorkerService> WorkerServices { get; set; } = new List<WorkerService>();
    }
}
