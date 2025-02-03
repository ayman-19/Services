using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.Update
{
	public sealed record UpdateUserCommand(Guid id, string name, string email, string phone) : IRequest<ResponseOf<UpdateUserResult>>;
}
