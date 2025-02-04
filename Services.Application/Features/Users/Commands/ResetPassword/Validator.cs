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
                .WithMessage(ValidationMessages.User.ValidMail)
                .NotEmpty()
                .WithMessage(ValidationMessages.User.EmailIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.User.EmailIsRequired);

            RuleFor(x => x.password)
                .NotEmpty()
                .WithMessage(ValidationMessages.User.PasswordIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.User.PasswordIsRequired);

            RuleFor(x => x.confirmPassword)
                .NotEmpty()
                .WithMessage(ValidationMessages.User.ConfirmPasswordIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.User.ConfirmPasswordIsRequired);

            RuleFor(x => x.confirmPassword)
                .Equal(x => x.password)
                .WithMessage(ValidationMessages.User.ComparePassword);
        }
    }
}
