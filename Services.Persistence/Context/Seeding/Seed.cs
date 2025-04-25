using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Domain.Models;
using Services.Persistence.Data;

namespace Services.Persistence.Context.Seeding
{
    public static class Seed
    {
        public static async Task SeedAsync(ServiceDbContext context, IConfiguration configuration)
        {
            if (!context.Set<Role>().Any())
            {
                context
                    .Set<Role>()
                    .AddRange(
                        Role.Create(nameof(UserType.Admin)),
                        Role.Create(nameof(UserType.Worker)),
                        Role.Create(nameof(UserType.Customer))
                    );
            }

            if (!context.Set<User>().Any(u => u.Email == configuration["Admin:gmail"]))
            {
                User admin = User.Create(
                    configuration["Admin:name"] ?? "",
                    configuration["Admin:gmail"] ?? "",
                    configuration["Admin:phone"] ?? "",
                    UserType.Admin
                );
                var roleId = await context
                    .Set<Role>()
                    .Where(r => r.Name == nameof(UserType.Admin))
                    .Select(r => r.Id)
                    .FirstAsync();

                admin.UserRoles.Add(new UserRole { RoleId = roleId });
                await context.Set<User>().AddAsync(admin);
            }

            //if (!context.Set<Permission>().Any())
            //{
            //    context
            //        .Set<Permission>()
            //        .AddRange(new Permission { Name = "Read" }, new Permission { Name = "Write" });
            //}

            await context.SaveChangesAsync();
        }
    }
}
