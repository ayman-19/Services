using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities;
using Services.Domain.Enums;

namespace Services.Persistence.Context.Configuration
{
    public sealed class DiscoundConfiguration : IEntityTypeConfiguration<Discound>
    {
        public void Configure(EntityTypeBuilder<Discound> builder)
        {
            builder.ToTable(nameof(Table.Discound), nameof(Schema.Service));
            builder.HasKey(t => t.Id);
        }
    }
}
