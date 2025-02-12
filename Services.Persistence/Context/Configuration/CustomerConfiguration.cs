using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities;
using Services.Domain.Enums;

namespace Services.Persistence.Context.Configuration
{
    public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(nameof(Table.Customer), nameof(Schema.Service));
            builder.HasKey(c => c.UserId);
        }
    }
}
