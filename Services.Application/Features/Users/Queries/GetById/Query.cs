using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Queries.GetById
{
	public sealed record GetUserQuery(Guid id) : IRequest<ResponseOf<GetUserResult>>;
}
