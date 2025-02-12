using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities;
using Services.Domain.Enums;

namespace Services.Persistence.Context.Configuration
{
    public sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable(nameof(Table.Branch), nameof(Schema.Service));
            builder.HasKey(t => t.Id);
        }
    }
}
