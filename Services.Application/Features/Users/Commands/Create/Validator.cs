﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Domain.Enums;
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
                scope.ServiceProvider.GetRequiredService<IServiceRepository>(),
                scope.ServiceProvider.GetRequiredService<ICategoryRepository>()
            );
        }

        private void ValidateRequest(
            IUserRepository userRepository,
            IServiceRepository serviceRepository,
            ICategoryRepository categoryRepository
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
                .WithMessage(ValidationMessages.Users.ValidEmail)
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
                .WithMessage(ValidationMessages.Users.EmailExists);

            RuleFor(x => x)
                .MustAsync(
                    async (request, cancellationToken) =>
                    {
                        if (request.UserType != UserType.Worker || request.ServiceId == null)
                        {
                            return true;
                        }

                        return await serviceRepository.IsAnyExistAsync(
                            s => s.Id == request.ServiceId,
                            cancellationToken
                        );
                    }
                )
                .WithMessage(ValidationMessages.Services.ServiceDoesNotExist)
                .When(x => x.UserType == UserType.Worker && x.ServiceId != null);

            RuleFor(x => x)
                .MustAsync(
                    async (request, cancellationToken) =>
                    {
                        if (request.UserType != UserType.Worker || request.CategoryId == null)
                        {
                            return true;
                        }

                        return await categoryRepository.IsAnyExistAsync(
                            s => s.Id == request.CategoryId,
                            cancellationToken
                        );
                    }
                )
                .WithMessage(ValidationMessages.Categories.CategoryDoesNotExist)
                .When(x => x.UserType == UserType.Worker && x.CategoryId != null);

            RuleFor(x => x)
                .Must((e, cancellationToken) => e.phone.ValidatePhoneNumber())
                .WithMessage(ValidationMessages.Users.PhoneNumberNotValid);
        }
    }
}
