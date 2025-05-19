using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities;
using Services.Domain.Enums;

namespace Services.Persistence.Context.Configuration
{
    public sealed class DiscountRuleConfiguration : IEntityTypeConfiguration<DiscountRule>
    {
        public void Configure(EntityTypeBuilder<DiscountRule> builder)
        {
            builder.ToTable(nameof(Table.DiscountRule), nameof(Schema.Service));
            builder.HasKey(t => t.Id);
        }
    }
}
