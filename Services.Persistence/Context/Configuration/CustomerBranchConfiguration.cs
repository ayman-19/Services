using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities;
using Services.Domain.Enums;

namespace Services.Persistence.Context.Configuration
{
    public sealed class CustomerBranchConfiguration : IEntityTypeConfiguration<CustomerBranch>
    {
        public void Configure(EntityTypeBuilder<CustomerBranch> builder)
        {
            builder.ToTable(nameof(Table.CustomerBranch), nameof(Schema.Service));
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.BranchId, x.CustomerId }).IsUnique(true);
        }
    }
}
