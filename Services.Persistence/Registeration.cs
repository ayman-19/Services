using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Services.Domain.Abstraction;
using Services.Domain.Models;
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
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IEmailSender, EmailSender>()
                .AddScoped<ITokenRepository, TokenRepository>()
                .AddScoped<IJWTManager, JWTManager>()
                .AddScoped<IBranchRepository, BranchRepository>()
                .AddScoped<IWorkerServiceRepository, WorkerServiceRepository>()
                .AddScoped<IWorkerRepository, WorkerRepository>()
                .AddScoped<IServiceRepository, ServiceRepository>();
            services.AddAuthentication();

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
            });
            services.AddQuartzHostedService(opt =>
            {
                opt.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}
