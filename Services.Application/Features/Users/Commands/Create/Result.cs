using System.Net;
using Services.Domain.Models;

namespace Services.Application.Features.Users.Commands.Create
{
    public sealed record CreateUserResult(
        Guid Id,
        string Name,
        string Email,
        DateTime CreatedOn,
        IEnumerable<string> roles
    )
    {
        public static implicit operator CreateUserResult(User user) =>
            new(
                user.Id,
                user.Name,
                user.Email,
                user.CreateOn,
                user.UserRoles.Any() ? user.UserRoles.Select(ur => ur.Role.Name) : []
            );
    }
}
