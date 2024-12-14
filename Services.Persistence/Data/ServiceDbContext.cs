using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Services.Persistence.Data
{
    public class ServiceDbContext : IdentityDbContext
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
