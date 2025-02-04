using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.Confirm
{
    public sealed record ConfirmUserCommand(string email, string code) : IRequest<Response>;
}
