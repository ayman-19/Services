using MediatR;
using Services.Domain.Entities;
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
        string confirmPassword,
        Guid? CategoryId,
        Guid? ServiceId,
        double? Experience,
        double? Price
    ) : IRequest<Response>
    {
        public static implicit operator Customer(CreateUserCommand command) =>
            new Customer()
            {
                User = User.Create(command.name, command.email, command.phone, command.UserType),
            };

        public static implicit operator Worker(CreateUserCommand command) =>
            new Worker()
            {
                Experience = command.Experience ?? 0,
                User = User.Create(command.name, command.email, command.phone, command.UserType),
                Status = Status.Pending,
            };
    }
}
