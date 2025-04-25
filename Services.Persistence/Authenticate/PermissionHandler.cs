using Microsoft.AspNetCore.Authorization;
using Services.Shared.Enums;

namespace Services.Persistence.Authenticate
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement
        )
        {
            if (!context.User.Identity!.IsAuthenticated)
                throw new Exception("Please Login Again!");

            var canAccess = await Task.FromResult(
                context.User.Claims.Any(t =>
                    t.Type == nameof(RoleClaims.Permissions) && t.Value == requirement.Permission
                )
            );

            if (canAccess)
                context.Succeed(requirement);
        }
    }
}
