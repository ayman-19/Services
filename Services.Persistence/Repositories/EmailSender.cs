using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Services.Persistence.Repositories
{
	public sealed class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			using (var smtp = new SmtpClient())
			{
				await smtp.ConnectAsync(_configuration["Email:Host"], int.Parse(_configuration["Email:Port"]!), true);
				await smtp.AuthenticateAsync(_configuration["Email:gmail"], _configuration["Email:password"]);
				var bodyBuilder = new BodyBuilder
				{
					HtmlBody = htmlMessage,
					TextBody = "Wellcome",
				};
				var sendMessage = new MimeMessage
				{
					Body = bodyBuilder.ToMessageBody(),
					Subject = subject
				};
				sendMessage.From.Add(new MailboxAddress("Services", _configuration["Email:gmail"]));
				sendMessage.To.Add(new MailboxAddress("", email));
				await smtp.SendAsync(sendMessage);
				smtp.Disconnect(true);
			}
			
		}
	}
}
