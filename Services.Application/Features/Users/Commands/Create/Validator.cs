using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Domain.Repositories;
using Services.Shared.Extentions;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.Create
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public CreateUserValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(
                scope.ServiceProvider.GetRequiredService<IUserRepository>(),
                scope.ServiceProvider.GetRequiredService<IServiceRepository>()
            );
        }

        private void ValidateRequest(
            IUserRepository userRepository,
            IServiceRepository serviceRepository
        )
        {
            RuleFor(x => x.name)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.NameIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.NameIsRequired);

            RuleFor(x => x.UserType)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.UserTypeIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.UserTypeIsRequired);

            RuleFor(x => x.phone)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.PhoneIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.PhoneIsRequired);

            RuleFor(x => x.email)
                .EmailAddress()
                .WithMessage(ValidationMessages.Users.ValidMail)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.EmailIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.EmailIsRequired);

            RuleFor(x => x.password)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.PasswordIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.PasswordIsRequired)
                .MinimumLength(8)
                .WithMessage(ValidationMessages.Users.MinLength)
                .MaximumLength(20)
                .WithMessage(ValidationMessages.Users.MaxLength)
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
                .Equal(x => x.password)
                .WithMessage(ValidationMessages.Users.ComparePassword);

            RuleFor(x => x)
                .MustAsync(
                    async (e, cancellationToken) =>
                        !await userRepository.IsAnyExistAsync(
                            user => user.Email.Trim().Equals(e.email),
                            cancellationToken
                        )
                )
                .WithMessage(ValidationMessages.Users.EmailIsExist);

            //RuleFor(x => x.ServiceId)
            //    .MustAsync(
            //        async (id, cancellationToken) =>
            //            await serviceRepository.IsAnyExistAsync(
            //                s => id == null || s.Id == id,
            //                cancellationToken
            //            )
            //    )
            //    .WithMessage(ValidationMessages.Service.ServiceNotExist);

            RuleFor(x => x)
                .Must((e, cancellationToken) => e.phone.ValidatePhoneNumber())
                .WithMessage(ValidationMessages.Users.PhoneNumberNotValid);
        }
    }
}
