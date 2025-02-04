using Services.Application;
using Services.Persistence;
using Services.Shared;

namespace Services.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.RegisterPersistenceDependancies(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            ConfigurationManager configuration = builder.Configuration;

            builder.Services.AddHttpContextAccessor();
            builder
                .Services.RegisterSharedDepenedncies()
                .RegisterApplicationDepenedncies(configuration)
                .RegisterMiddlewares()
                .RegisterPersistenceDependancies(configuration);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowAll",
                    policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.RegisterAllEndpoints();
            app.UseCors("AllowAll");

            app.Run();
        }
    }
}
