using System.Net;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Services.Domain.Abstraction;
using Services.Shared.Context;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Supports.Contactus;

public sealed class ContactusHandler(
    IJobs jobs,
    IUserContext userContext,
    IConfiguration configuration
) : IRequestHandler<ContactusCommand, Response>
{
    public async Task<Response> Handle(
        ContactusCommand request,
        CancellationToken cancellationToken
    )
    {
        var context = userContext.HttpContext;
        var ip = context.Connection.RemoteIpAddress?.ToString();
        var method = context.Request.Method;
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var host = context.Request.Host.ToString();
        var path = context.Request.Path;
        var url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";

        var queryString = string.Join(
            ", ",
            context.Request.Query.Select(q => $"{q.Key} - {q.Value}")
        );

        var contentType = context.Request.ContentType;

        var headers = string.Join(
            ", ",
            context.Request.Headers.Select(h => $"{h.Key} - {h.Value}")
        );

        var isAuthenticated = context.User.Identity?.IsAuthenticated ?? false;

        var token =
            context
                .Request.Headers["Authorization"]
                .FirstOrDefault(s => s.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                ?.Substring("Bearer ".Length)
                .Trim() ?? string.Empty;

        var userId = userContext.UserId.Exist ? userContext.UserId.Value : string.Empty;
        var userType = userContext.UserType.Exist ? userContext.UserType.Value : string.Empty;

        var alarm = new StringBuilder();
        void AppendLine(string label, string? value)
        {
            alarm.AppendLine($"{label}: {value ?? "N/A"}");
            alarm.AppendLine();
        }

        AppendLine("IP", ip);
        AppendLine("METHOD", method);
        AppendLine("USER AGENT", userAgent);
        AppendLine("HOST", host);
        AppendLine("PATH", path);
        AppendLine("URL", url);
        AppendLine("QUERY", queryString);
        AppendLine("CONTENT TYPE", contentType);
        AppendLine("HEADERS", headers);
        AppendLine("AUTHENTICATED", isAuthenticated.ToString());
        AppendLine("TOKEN", token);
        AppendLine("USER ID", userId);
        AppendLine("USER TYPE", userType);
        AppendLine("NAME", request.Name);
        AppendLine("GMAIL", request.Gmail);
        AppendLine("PHONE", request.Phone);
        AppendLine("ADDRESS", request.Address);
        AppendLine("CONTENT", request.Content);

        await jobs.SendEmailByJobAsync(configuration["Email:gmail"] ?? "", alarm.ToString());

        return new Response
        {
            Message = ValidationMessages.Success,
            Success = true,
            StatusCode = (int)HttpStatusCode.OK,
        };
    }
}
