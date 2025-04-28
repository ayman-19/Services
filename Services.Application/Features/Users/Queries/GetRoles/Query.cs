using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Queries.GetRoles
{
    public sealed record GetRolesQuery()
        : IRequest<ResponseOf<IReadOnlyCollection<GetRolesResult>>>;
}
