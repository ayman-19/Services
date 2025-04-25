using MediatR;
using Services.Shared.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.Login
{
    public sealed record LoginUserCommand(
        string email,
        string password,
        double Latitude,
        double Longitude
    ) : IRequest<ResponseOf<LoginUserResult>>;
}
