using Services.Domain.Abstraction;

namespace Services.Domain.Entities
{
    public sealed record Branch : ITrackableCreate, ITrackableUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Langitude { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }

        public void UpdaeBranch(string name, string description, double latitude, double langitude)
        {
            Name = name;
            Latitude = latitude;
            Langitude = langitude;
            Description = description;
        }

        public ICollection<WorkerService> WorkerServices { get; set; }
        public ICollection<CustomerBranch> CustomerBranchs { get; set; }

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;

        public void SetUpdateOn() => UpdateOn = DateTime.UtcNow;

        public Branch()
        {
            WorkerServices = new HashSet<WorkerService>();
            CustomerBranchs = new HashSet<CustomerBranch>();
        }
    }
}
