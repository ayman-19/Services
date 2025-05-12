using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Enums;
using Services.Domain.Models;

namespace Services.Persistence.Context.Configuration
{
    public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Table.Role), nameof(Schema.Identity));
            builder.HasKey(k => k.Id);
            builder.HasIndex(ind => ind.Name).IsUnique(true);
        }
    }
}
