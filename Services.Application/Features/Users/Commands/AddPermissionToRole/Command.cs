using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.AddPermissionToRole
{
    public sealed record AddPermissionToRoleCommand(Guid RoleId, Guid PermissionId)
        : IRequest<Response>;
}
