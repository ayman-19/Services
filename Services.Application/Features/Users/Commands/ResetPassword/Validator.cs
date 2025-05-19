using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Repositories;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.ResetPassword
{
    public sealed class ResetPasswordUserValidator : AbstractValidator<ResetPasswordUserCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public ResetPasswordUserValidator(IServiceProvider serviceProvider)
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
                .EmailAddress()
                .WithMessage(ValidationMessages.Users.ValidEmail)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.EmailIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.EmailIsRequired);

            RuleFor(x => x.password)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.PasswordIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.PasswordIsRequired);

            RuleFor(x => x.confirmPassword)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.ConfirmPasswordIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.ConfirmPasswordIsRequired);

            RuleFor(x => x.confirmPassword)
                .Equal(x => x.password)
                .WithMessage(ValidationMessages.Users.ComparePassword);
        }
    }
}
