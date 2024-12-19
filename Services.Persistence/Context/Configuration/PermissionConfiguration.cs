using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Enums;
using Services.Domain.Models;

namespace Services.Persistence.Context.Configuration
{
    public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(nameof(Table.Permission), nameof(Schema.Identity));
            builder.HasKey(k => k.Id);
            builder.HasIndex(ind => ind.Name).IsUnique(true);
        }
    }
}
