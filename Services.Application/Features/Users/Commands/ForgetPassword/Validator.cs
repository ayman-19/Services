using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Repositories;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.ForgetPassword
{
    public sealed class ForgetPasswordUserValidator : AbstractValidator<ForgetPasswordUserCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public ForgetPasswordUserValidator(IServiceProvider serviceProvider)
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

            RuleFor(x => x)
                .MustAsync(
                    async (e, cancellationToken) =>
                        await userRepository.IsAnyExistAsync(
                            user => user.Email.Trim().Equals(e.email),
                            cancellationToken
                        )
                )
                .WithMessage(ValidationMessages.User.EmailIsNotExist);
        }
    }
}
