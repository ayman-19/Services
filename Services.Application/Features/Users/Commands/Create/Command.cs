using MediatR;
using Services.Domain.Enums;
using Services.Domain.Models;
using Services.Shared.Responses;

namespace Services.Application.Features.Users.Commands.Create
{
    public sealed record CreateUserCommand(
        string name,
        string email,
        string phone,
        UserType UserType,
        string password,
        string confirmPassword
    ) : IRequest<ResponseOf<CreateUserResult>>
    {
        public static implicit operator User(CreateUserCommand command) =>
            User.Create(command.name, command.email, command.phone, command.UserType);
    }
}
