namespace Services.Domain.Abstraction
{
    public interface IJobs
    {
        Task SendEmailByJobAsync(string email, string code);
        Task RateWorkersAsync(Guid WorkerId, Guid ServiceId, Guid CustomerId);
    }
}
