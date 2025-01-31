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
			RuleFor(x => x.emailOrPhone)
				.NotEmpty()
				.WithMessage(ValidationMessages.User.EmailOrPhoneIsRequired)
				.NotNull()
				.WithMessage(ValidationMessages.User.EmailOrPhoneIsRequired);

			RuleFor(x => x.password)
				.NotEmpty()
				.WithMessage(ValidationMessages.User.PasswordIsRequired)
				.NotNull()
				.WithMessage(ValidationMessages.User.PasswordIsRequired);

			RuleFor(x => x.emailOrPhone)
				.MustAsync(async (eOp, CancellationToken) => await userRepository.EmailOrPhoneIsExist(eOp))
				.WithMessage(ValidationMessages.User.EmailOrPhoneNumberNotExist);
		}
	}
}
