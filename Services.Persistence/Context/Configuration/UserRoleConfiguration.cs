using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities;
using Services.Domain.Enums;

namespace Services.Persistence.Context.Configuration
{
    public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(nameof(Table.UserRole), nameof(Schema.Identity));
            builder.HasKey(k => k.Id);
            builder.HasIndex(ind => new { ind.RoleId, ind.UserId }).IsUnique(true);
        }
    }
}
