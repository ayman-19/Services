using Services.Domain.Abstraction;

namespace Services.Domain.Entities
{
    public sealed record Branch : ITrackableCreate, ITrackableUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Langitude { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public void UpdaeBranch(string name ,double latitude,double langitude)
        {
            Name = name;    
            Latitude = latitude;
            Langitude = langitude;
        }
        public ICollection<WorkerService> WorkerServices { get; set; }
        public ICollection<CustomerBranch> CustomerBranchs { get; set; }

        public void SetCreateOn() => CreateOn = DateTime.UtcNow;

        public void SetUpdateOn() => UpdateOn = DateTime.UtcNow;
    }
}
