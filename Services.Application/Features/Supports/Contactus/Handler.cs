using System.Net;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Services.Domain.Abstraction;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Supports.Contactus
{
    public sealed class ContactusHandler(IJobs jobs, IConfiguration configuration)
        : IRequestHandler<ContactusCommand, Response>
    {
        public async Task<Response> Handle(
            ContactusCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var alarm = new StringBuilder();
                alarm.AppendLine($"NAME: {request.Name}\n");
                alarm.AppendLine($"GMAIL: {request.Gmail}\n");
                alarm.AppendLine($"PHONE: {request.Phone}\n");
                alarm.AppendLine($"ADDRESS: {request.Address}\n");
                alarm.AppendLine($"CONTENT: {request.Content}\n");
                await jobs.SendEmailByJobAsync(
                    configuration["Email:gmail"] ?? "",
                    alarm.ToString()
                );
                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
