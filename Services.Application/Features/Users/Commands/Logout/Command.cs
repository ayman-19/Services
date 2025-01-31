using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.Logout
{
	public sealed record LogoutUserCommand() : IRequest<Response>;
}
