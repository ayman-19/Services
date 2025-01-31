using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.Delete
{
	public sealed record DeleteUserCommand(Guid userId) : IRequest<Response>;
}
