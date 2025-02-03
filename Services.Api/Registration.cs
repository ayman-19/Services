using Services.Api.Abstraction;

namespace Services.Api
{
	public static class Registration
	{
		public static void RegisterAllEndpoints(this IEndpointRouteBuilder app)
		{
			IEnumerable<IEndpoint>? endpoints = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assemply => assemply.GetTypes())
				.Where(type => typeof(IEndpoint).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
				.Select(Activator.CreateInstance)
				.Cast<IEndpoint>();

			foreach (IEndpoint endpoint in endpoints)
				endpoint.RegisterEndpoints(app);
		}
	}
}
