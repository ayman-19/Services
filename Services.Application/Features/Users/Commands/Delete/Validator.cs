using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Repositories;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.Delete
{
    public sealed class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteUserValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IUserRepository>());
        }

        private void ValidateRequest(IUserRepository userRepository)
        {
            RuleFor(user => user.userId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Services.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Services.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await userRepository.IsAnyExistAsync(user => user.Id == id)
                )
                .WithMessage(ValidationMessages.Services.ServiceDoesNotExist);
        }
    }
}
