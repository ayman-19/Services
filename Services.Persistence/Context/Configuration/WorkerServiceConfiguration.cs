using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Domain.Entities;
using Services.Domain.Enums;

namespace Services.Persistence.Context.Configuration
{
    public sealed class WorkerServiceConfiguration : IEntityTypeConfiguration<WorkerService>
    {
        public void Configure(EntityTypeBuilder<WorkerService> builder)
        {
            builder.ToTable(nameof(Table.WorkerService), nameof(Schema.service));

            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new {x.ServiceId,x.BranchId,x.WorkerId}).IsUnique(true);
        }
    }
}
