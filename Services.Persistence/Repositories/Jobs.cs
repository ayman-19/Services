using Quartz;
using Services.Domain.Abstraction;
using Services.Persistence.BackgroundJobs;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Services.Persistence.Repositories
{
    public sealed class Jobs : IJobs
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public Jobs(ISchedulerFactory schedulerFactory) => _schedulerFactory = schedulerFactory;

        public async Task RateWorkersAsync(Guid WorkerId, Guid ServiceId)
        {
            var jobKey = new JobKey(nameof(RateJob));
            var scheduler = await _schedulerFactory.GetScheduler();
            if (!await scheduler.CheckExists(jobKey))
            {
                var jobData = new JobDataMap
                {
                    { "WorkerId", WorkerId },
                    { "ServiceId", ServiceId },
                };

                var jobDetail = JobBuilder
                    .Create<RateJob>()
                    .WithIdentity(jobKey)
                    .UsingJobData(jobData)
                    .Build();

                var trigger = TriggerBuilder
                    .Create()
                    .ForJob(jobKey)
                    .StartNow()
                    .WithSimpleSchedule()
                    .Build();

                await scheduler.ScheduleJob(jobDetail, trigger);
            }
        }

        public async Task SendEmailByJobAsync(string email, string code)
        {
            var jobKey = new JobKey(nameof(SendEmailJob));
            var scheduler = await _schedulerFactory.GetScheduler();
            if (!await scheduler.CheckExists(jobKey))
            {
                var jobData = new JobDataMap { { "Email", email }, { "Code", code } };

                var jobDetail = JobBuilder
                    .Create<SendEmailJob>()
                    .WithIdentity(jobKey)
                    .UsingJobData(jobData)
                    .Build();

                var trigger = TriggerBuilder
                    .Create()
                    .ForJob(jobKey)
                    .StartNow()
                    .WithSimpleSchedule()
                    .Build();

                await scheduler.ScheduleJob(jobDetail, trigger);
            }
        }
    }
}
