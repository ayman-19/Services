using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities;
using Services.Domain.Enums;

namespace Services.Persistence.Context.Configuration
{
    public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .ToTable(nameof(Table.Category))
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentId)
                .IsRequired(false);
            builder.Property(c => c.ParentId).HasDefaultValue(null).IsRequired(false);
            builder.HasKey(k => k.Id);
            builder.HasIndex(x => x.Name).IsUnique(true);
        }
    }
}
