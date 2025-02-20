using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Services.Persistence.Data
{
    public sealed class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
            : base(options) { } /*=> Database.EnsureCreated();*/

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
