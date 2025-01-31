using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Repositories;
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
			ValidateRequest(scope.ServiceProvider.GetRequiredService<IUserRepository>());
		}
		private void ValidateRequest(IUserRepository userRepository)
		{
			RuleFor(x => x.name)
				.NotEmpty()
				.WithMessage(ValidationMessages.User.NameIsRequired)
				.NotNull()
				.WithMessage(ValidationMessages.User.NameIsRequired);

			RuleFor(x => x.phone)
				.NotEmpty()
				.WithMessage(ValidationMessages.User.PhoneIsRequired)
				.NotNull()
				.WithMessage(ValidationMessages.User.PhoneIsRequired);

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

			RuleFor(x => x)
				.MustAsync(async (e, cancellationToken) => !await userRepository.IsAnyExistAsync(user => user.Email.Trim().Equals(e.email),cancellationToken))
				.WithMessage(ValidationMessages.User.EmailIsExist);

		}

	}
}
