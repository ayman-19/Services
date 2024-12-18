namespace Services.Domain.Abstraction
{
    public interface ITrackableUpdate
    {
        public DateTime? UpdateOn { get; set; }
        public void SetUpdateOn();
    }
}
