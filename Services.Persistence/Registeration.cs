using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Domain.Repositories;
using Services.Persistence.Context.Interceptor;
using Services.Persistence.Data;
using Services.Persistence.Repositories;

namespace Services.Persistence
{
    public static class Registeration
    {
        public static IServiceCollection RegisterPersistenceDependancies(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContextPool<ServiceDbContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("SERVICE_CONNECTIONSTRING"));
                cfg.AddInterceptors(new OnSaveChangesInterceptor());
            });

            services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
