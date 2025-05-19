using Quartz;
using Services.Domain.Abstraction;

namespace Services.Persistence.BackgroundJobs
{
    public sealed record RateJob(
        IWorkerServiceRepository WorkerServiceRepository,
        IPointRepository PointRepository
    ) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap jobDate = context.JobDetail.JobDataMap;
            string workerId = jobDate.GetString("WorkerId")!;
            string serviceId = jobDate.GetString("ServiceId")!;
            string customerId = jobDate.GetString("CustomerId")!;
            await WorkerServiceRepository.RateWorkersAsync(
                Guid.Parse(workerId),
                Guid.Parse(serviceId)
            );
            await PointRepository.UpdateForCustomerAsync(Guid.Parse(customerId));
        }
    }
}
