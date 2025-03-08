using Services.Domain.Abstraction;

namespace Services.Domain.Entities
{
    public sealed record Service : ITrackableCreate, ITrackableUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Duration { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }

        public void UpdateService(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public ICollection<WorkerService> WorkerServices { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;

        public void SetUpdateOn() => UpdateOn = DateTime.UtcNow;

        public Service() => WorkerServices = new List<WorkerService>();
    }
}
