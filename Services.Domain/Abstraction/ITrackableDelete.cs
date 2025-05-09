namespace Services.Domain.Abstraction
{
    public interface ITrackableDelete
    {
        public DateTime? DeleteOn { get; set; }
        public void SetDeleteOn();
    }
}
