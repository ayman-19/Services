﻿using Quartz;
using Services.Domain.Abstraction;
using Services.Persistence.BackgroundJobs;

namespace Services.Persistence.Repositories
{
    public sealed class Jobs : IJobs
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public Jobs(ISchedulerFactory schedulerFactory) => _schedulerFactory = schedulerFactory;

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
