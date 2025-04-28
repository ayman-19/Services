using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Api.Middlewares;
using Services.Application;
using Services.Domain.Models;
using Services.Persistence;
using Services.Persistence.Context.Seeding;
using Services.Persistence.Data;
using Services.Shared;
using Services.Shared.Settings;

namespace Services.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Service.", Version = "v1" });
                options.DescribeAllParametersInCamelCase();
                options.InferSecuritySchemes();
            });

            builder.Services.AddSwaggerGen(o =>
            {
                o.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer",
                    }
                );
                o.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer",
                                },
                            },
                            new string[] { }
                        },
                    }
                );
            });
            builder.Services.AddEndpointsApiExplorer();
            string _cors = "services";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    _cors,
                    policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            });
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            ConfigurationManager configuration = builder.Configuration;

            builder.Services.AddHttpContextAccessor();
            builder
                .Services.RegisterSharedDepenedncies()
                .RegisterPersistenceDependancies(configuration)
                .RegisterApplicationDepenedncies(configuration)
                .RegisterMiddlewares();

            builder.Services.AddEndpointsApiExplorer();

            builder
                .Services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = JwtSettings.Issuer,
                        ValidAudience = JwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(JwtSettings.Key)
                        ),
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            WebApplication app = builder.Build();
            app.UseMiddleware<ExceptionHandler>();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();
            app.UseAuthentication();
            app.UseMiddleware<TokenValidation>();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseCors(_cors);
            app.MapOpenApi();
            app.UseHttpsRedirection();
            app.MapControllers();

            app.RegisterAllEndpoints();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
                await Seed.SeedAsync(
                    dbContext,
                    builder.Configuration,
                    scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>()
                );
            }

            app.Run();
        }
    }
}
