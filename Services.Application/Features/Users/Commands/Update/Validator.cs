using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Repositories;
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

		}

	}
}
