using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.ResetPassword
{
    public sealed record ResetPasswordUserCommand(
        string email,
        string password,
        string confirmPassword,
        string code
    ) : IRequest<Response>;
}
