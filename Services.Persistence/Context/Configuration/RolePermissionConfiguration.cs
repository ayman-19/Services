using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Enums;
using Services.Domain.Models;

namespace Services.Persistence.Context.Configuration
{
    public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable(nameof(Table.RolePermission), nameof(Schema.Identity));
            builder.HasKey(k => k.Id);
            builder.HasIndex(ind => new { ind.RoleId, ind.PermissionId }).IsUnique(true);
        }
    }
}
