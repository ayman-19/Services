using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.Login
{
    public sealed record LoginUserCommand(string email, string password)
        : IRequest<ResponseOf<LoginUserResult>>;
}
