using MediatR;
using Services.Domain.Models;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.AddPermissionToRole
{
    public sealed record AddPermissionToRoleCommand(Guid RoleId, Guid PermissionId)
        : IRequest<Response>
    {
        public static implicit operator RolePermission(AddPermissionToRoleCommand command) =>
            RolePermission.Create(command.RoleId, command.PermissionId);
    }
}
