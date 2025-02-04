using Services.Domain.Models;

namespace Services.Application.Features.Users.Commands.ForgetPassword
{
    public sealed record ForgetPasswordUserResult(Guid Id, string Name, string Email)
    {
        public static implicit operator ForgetPasswordUserResult(User user) =>
            new(user.Id, user.Name, user.Email);
    }
}
