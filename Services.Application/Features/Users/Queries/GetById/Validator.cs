using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Features.Users.Commands.Delete;
using Services.Domain.Repositories;
using Services.Shared.ValidationMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Features.Users.Queries.GetById
{
	public sealed class GetUserValidator : AbstractValidator<GetUserQuery>
	{
		private readonly IServiceProvider _serviceProvider;

		public GetUserValidator(IServiceProvider serviceProvider)
		{
			RuleLevelCascadeMode = CascadeMode.Stop;
			ClassLevelCascadeMode = CascadeMode.Stop;
			_serviceProvider = serviceProvider;
			var scope = _serviceProvider.CreateScope();
			ValidateRequest(scope.ServiceProvider.GetRequiredService<IUserRepository>());
		}
		private void ValidateRequest(IUserRepository userRepository)
		{
			RuleFor(user => user.id)
				.NotEmpty()
				.WithMessage(ValidationMessages.User.IdIsRequired)
				.NotNull()
				.WithMessage(ValidationMessages.User.IdIsRequired)
				.MustAsync(async (id, CancellationToken) => await userRepository.IsAnyExistAsync(user => user.Id == id))
				.WithMessage(ValidationMessages.User.UserNotExist);
		}
	}
}
