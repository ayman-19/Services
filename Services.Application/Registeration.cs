using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Behaviores;
using Services.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application
{
	public static class Registeration
	{
		public static IServiceCollection RegisterApplicationDepenedncies(
			this IServiceCollection services,
			IConfiguration configuration
		)
		{
			services.AddMediatR(cfg =>
				cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
			);

			services.AddValidatorsFromAssembly(typeof(Registeration).Assembly);
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
			services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

			return services;
		}
	}
}
