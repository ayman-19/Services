using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Domain.Models;
using Services.Persistence.Data;
using Services.Shared.Enums;

namespace Services.Persistence.Context.Seeding
{
    public static class Seed
    {
        public static async Task SeedAsync(
            ServiceDbContext context,
            IConfiguration configuration,
            IPasswordHasher<User> passwordHasher
        )
        {
            if (!context.Set<Permission>().Any())
            {
                var permissions = Enum.GetValues(typeof(Permissions))
                    .Cast<Permissions>()
                    .Select(value => Permission.Create(value.ToString()))
                    .ToList();
                context.Set<Permission>().AddRange(permissions);
            }

            if (!context.Set<Role>().Any())
            {
                context
                    .Set<Role>()
                    .AddRange(
                        Role.Create(Guid.NewGuid(), nameof(UserType.Admin)),
                        Role.Create(Guid.NewGuid(), nameof(UserType.Worker)),
                        Role.Create(Guid.NewGuid(), nameof(UserType.Customer))
                    );
            }

            if (!context.Set<User>().Any(u => u.Email == configuration["Admin:gmail"]))
            {
                User admin = User.Create(
                    configuration["Admin:name"],
                    configuration["Admin:gmail"],
                    configuration["Admin:phone"],
                    UserType.Admin
                );
                admin.HashPassword(passwordHasher, configuration["Admin:password"]);
                admin.ConfirmAccount = true;
                admin.Branch = new() { Langitude = 0, Latitude = 0 };
                //var role = await context
                //    .Set<Role>()
                //    .AsNoTracking()
                //    .FirstOrDefaultAsync(r => r.Name == nameof(UserType.Admin));
                //if (role != null)
                //    admin.UserRoles.Add(new UserRole { RoleId = role.Id });
                await context.Set<User>().AddAsync(admin);
            }
            await context.SaveChangesAsync();
        }
    }
}
