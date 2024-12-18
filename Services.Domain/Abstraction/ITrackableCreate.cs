namespace Services.Domain.Abstraction
{
    public interface ITrackableCreate
    {
        public DateTime CreateOn { get; set; }
        public void SetCreateOn();
    }
}
