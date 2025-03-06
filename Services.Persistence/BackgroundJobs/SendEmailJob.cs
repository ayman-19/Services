using Microsoft.AspNetCore.Identity.UI.Services;
using Quartz;
using Services.Shared.ValidationMessages;

namespace Services.Persistence.BackgroundJobs
{
    public class SendEmailJob : IJob
    {
        private readonly IEmailSender _emailSender;

        public SendEmailJob(IEmailSender emailSender) => _emailSender = emailSender;

        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap jobDate = context.JobDetail.JobDataMap;
            string email = jobDate.GetString("Email")!;
            string code = jobDate.GetString("Code")!;
            await _emailSender.SendEmailAsync(
                email,
                ValidationMessages.User.ConfirmEmail,
                $"To Confirm Email Code: <h3>{code}</h3>"
            );
        }
    }
}
