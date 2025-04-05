using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities;
using Services.Domain.Enums;

namespace Services.Persistence.Context.Configuration
{
    public sealed class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable(nameof(Table.Discound), nameof(Schema.Service));
            builder.HasKey(t => t.Id);
        }
    }
}
