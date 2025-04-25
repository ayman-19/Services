namespace Services.Application.Features.Users.Commands.AssignToRole
{
    public sealed record AssignToRoleResult(Guid UserId, Guid RoleId, string RoleName);
}
