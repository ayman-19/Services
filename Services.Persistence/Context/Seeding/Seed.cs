using System.Linq;
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
                        Role.Create(nameof(UserType.Admin)),
                        Role.Create(nameof(UserType.Worker)),
                        Role.Create(nameof(UserType.Customer))
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

                var roleId = await context
                    .Set<Role>()
                    .AsNoTracking()
                    .Where(r => r.Name == nameof(UserType.Admin))
                    .Select(r => r.Id)
                    .FirstAsync();
                if (roleId != Guid.Empty)
                    admin.UserRoles.Add(new UserRole { RoleId = roleId });

                var permissionIds = context
                    .Set<Permission>()
                    .Where(p =>
                        p.Name == nameof(Permissions.AddPermissionToRole)
                        || p.Name == nameof(Permissions.AssignToRole)
                    )
                    .Select(p => p.Id);

                await context
                    .Set<RolePermission>()
                    .AddRangeAsync(permissionIds.Select(p => RolePermission.Create(roleId, p)));

                await context.Set<User>().AddAsync(admin);
            }
            await context.SaveChangesAsync();
        }
    }
}
