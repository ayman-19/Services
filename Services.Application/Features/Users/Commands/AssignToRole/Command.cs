using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.AssignToRole
{
    public sealed record AssignToRoleCommand(Guid UserId, Guid RoleId)
        : IRequest<ResponseOf<AssignToRoleResult>>;
}
