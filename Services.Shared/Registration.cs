using Microsoft.Extensions.DependencyInjection;
using Services.Shared.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Shared
{
	public static class Registeration
	{
		public static IServiceCollection RegisterSharedDepenedncies(
			this IServiceCollection services
		)
		{
			services.AddScoped<IUserContext, UserContext>();
			return services;
		}
	}
}
