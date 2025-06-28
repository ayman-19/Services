using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.UpdatePassword
{
    public sealed record UpdatePasswordCommand(
        Guid Id,
        string oldPassword,
        string newPassword,
        string confirmPassword
    ) : IRequest<Response>;
}
