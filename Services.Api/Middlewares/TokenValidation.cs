using System.Text.Json;
using Services.Domain.Abstraction;
using Services.Shared.Responses;

namespace Services.Api.Middlewares;

public sealed class TokenValidation(ITokenRepository tokenRepository) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var path = context.Request?.Path.Value?.ToLower() ?? string.Empty;

        if (
            !path.EndsWith("loginasync")
            && !path.EndsWith("forgetpasswordasync")
            && !path.EndsWith("resetpasswordasync")
            && !path.EndsWith("registerasync")
            && !path.EndsWith("confirmasync")
            && context.User.Identity?.IsAuthenticated == true
        )
        {
            try
            {
                var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                if (
                    string.IsNullOrWhiteSpace(authorizationHeader)
                    || !authorizationHeader.StartsWith(
                        "Bearer ",
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                    throw new UnauthorizedAccessException(
                        "Authorization header is missing or invalid."
                    );

                var token = authorizationHeader["Bearer ".Length..].Trim();

                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("Token is empty or missing.");

                var validToken = await tokenRepository.IsAnyExistAsync(t => t.Content == token);

                if (!validToken)
                    throw new UnauthorizedAccessException(
                        "Invalid or expired token. Please log in again."
                    );
            }
            catch (UnauthorizedAccessException ex)
            {
                await WriteErrorResponse(context, StatusCodes.Status401Unauthorized, ex.Message);
                return;
            }
            catch (Exception)
            {
                await WriteErrorResponse(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred."
                );
                return;
            }
        }

        await next(context);
    }

    private static async Task WriteErrorResponse(
        HttpContext context,
        int statusCode,
        string message
    )
    {
        if (!context.Response.HasStarted)
        {
            var response = new Response
            {
                Message = message,
                StatusCode = statusCode,
                Success = false,
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
