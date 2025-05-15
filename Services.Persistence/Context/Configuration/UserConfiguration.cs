using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Enums;
using Services.Domain.Models;

namespace Services.Persistence.Context.Configuration
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(Table.User), nameof(Schema.Identity));
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Email).IsUnique(true);
            builder.HasIndex(x => x.Name);
            builder.Navigation(User => User.Token);
            builder.Navigation(User => User.UserRoles);
        }
    }
}
