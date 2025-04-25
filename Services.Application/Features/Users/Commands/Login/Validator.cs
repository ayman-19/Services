using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Repositories;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.Login
{
    public sealed class LoginUserValidator : AbstractValidator<LoginUserCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public LoginUserValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IUserRepository>());
        }

        private void ValidateRequest(IUserRepository userRepository)
        {
            RuleFor(x => x.email)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.EmailOrPhoneIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.EmailOrPhoneIsRequired);

            RuleFor(x => x.password)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.PasswordIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.PasswordIsRequired);

            RuleFor(x => x.email)
                .MustAsync(
                    async (e, CancellationToken) =>
                        await userRepository.IsAnyExistAsync(email => email.Email == e)
                )
                .WithMessage(ValidationMessages.Users.EmailIsNotExist);
        }
    }
}
