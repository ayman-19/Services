using Quartz;

namespace Services.Persistence.BackgroundJobs
{
    public sealed record RateJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
