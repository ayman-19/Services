using Services.Domain.Models;

namespace Services.Application.Features.Users.Commands.Update
{
    public sealed record UpdateUserResult(
        Guid Id,
        string Name,
        string Email,
        DateTime CreatedOn,
        string Content,
        IEnumerable<string> roles
    )
    {
        public static implicit operator UpdateUserResult(User user) =>
            new(
                user.Id,
                user.Name,
                user.Email,
                user.CreateOn,
                user.Token.Content,
                user.UserRoles.Select(ur => ur.Id.ToString())
            );
    }
}
