using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.ForgetPassword
{
    public sealed record ForgetPasswordUserCommand(string email)
        : IRequest<ResponseOf<ForgetPasswordUserResult>>;
}
