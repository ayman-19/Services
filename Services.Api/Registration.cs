using Services.Api.Abstraction;
using Services.Api.Middlewares;

namespace Services.Api
{
    public static class Registration
    {
        public static IServiceCollection RegisterMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<ExceptionHandler>();
            return services;
        }

        public static void RegisterAllEndpoints(this IEndpointRouteBuilder app)
        {
            IEnumerable<IEndpoint>? endpoints = AppDomain
                .CurrentDomain.GetAssemblies()
                .SelectMany(assemply => assemply.GetTypes())
                .Where(type =>
                    typeof(IEndpoint).IsAssignableFrom(type)
                    && !type.IsInterface
                    && !type.IsAbstract
                )
                .Select(Activator.CreateInstance)
                .Cast<IEndpoint>();

            foreach (IEndpoint endpoint in endpoints)
                endpoint.RegisterEndpoints(app);
        }
    }
}
