using Quartz;

namespace Services.Persistence.BackgroundJobs
{
    public sealed record BackgroundJobRate : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
