using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Repositories;
using Services.Shared.Extentions;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.Update
{
    public class CreateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public CreateUserValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IUserRepository>());
        }

        private void ValidateRequest(IUserRepository userRepository)
        {
            RuleFor(x => x.name)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.NameIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.NameIsRequired);

            RuleFor(x => x.phone)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.PhoneIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.PhoneIsRequired);

            RuleFor(x => x.email)
                .EmailAddress()
                .WithMessage(ValidationMessages.Users.ValidEmail)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.EmailIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.EmailIsRequired);

            RuleFor(x => x)
                .Must((e, cancellationToken) => e.phone.ValidatePhoneNumber())
                .WithMessage(ValidationMessages.Users.PhoneNumberNotValid);
        }
    }
}
