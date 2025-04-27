using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Queries.GetPermissions
{
    public sealed record GetPermissionsQuery()
        : IRequest<ResponseOf<IReadOnlyCollection<GetPermissionsResult>>>;
}
