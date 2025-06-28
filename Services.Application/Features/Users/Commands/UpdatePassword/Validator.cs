using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Repositories;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.UpdatePassword
{
    public sealed class UpdatePasswordValidator : AbstractValidator<UpdatePasswordCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdatePasswordValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IUserRepository>());
        }

        private void ValidateRequest(IUserRepository userRepository)
        {
            RuleFor(user => user.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await userRepository.IsAnyExistAsync(user => user.Id == id)
                );

            RuleFor(x => x.oldPassword)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.PasswordIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.PasswordIsRequired);

            RuleFor(x => x.newPassword)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.PasswordIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.PasswordIsRequired)
                .MinimumLength(8)
                .WithMessage(ValidationMessages.Users.MinPasswordLength)
                .MaximumLength(20)
                .WithMessage(ValidationMessages.Users.MaxPasswordLength)
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d")
                .WithMessage("Password must contain at least one digit.")
                .Matches(@"[\W_]")
                .WithMessage(
                    "Password must contain at least one special character (!@#$%^&* etc.)."
                );

            RuleFor(x => x.confirmPassword)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.ConfirmPasswordIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.ConfirmPasswordIsRequired);

            RuleFor(x => x.confirmPassword)
                .Equal(x => x.newPassword)
                .WithMessage(ValidationMessages.Users.ComparePassword);
        }
    }
}
