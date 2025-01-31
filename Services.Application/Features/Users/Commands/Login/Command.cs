using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.Login
{
	public sealed record LoginUserCommand(string emailOrPhone, string password) : IRequest<Response>;
}
