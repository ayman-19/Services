using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities;
using Services.Domain.Enums;

namespace Services.Persistence.Context.Configuration
{
    public sealed class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable(nameof(Table.Service), nameof(Schema.Service));
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.Name, x.Description }).IsUnique(true);
        }
    }
}
