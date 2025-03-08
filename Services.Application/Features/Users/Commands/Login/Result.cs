using Services.Domain.Enums;
using Services.Domain.Models;

namespace Services.Application.Features.Users.Commands.Login
{
    public sealed record LoginUserResult(Guid userId, UserType type, string content)
    {
        public static implicit operator LoginUserResult(User user) =>
            new(user.Id, user.UserType, user.Token.Content);
    }
}
