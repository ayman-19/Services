using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Services.Domain.Abstraction;

namespace Services.Persistence.Context.Interceptor
{
    public sealed class OnSaveChangesInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default
        )
        {
            if (eventData.Context == null)
                return ValueTask.FromResult(result);

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is { State: EntityState.Added, Entity: ITrackableCreate createEntity })
                    createEntity.SetCreateOn();
                else if (
                    entry is { State: EntityState.Modified, Entity: ITrackableUpdate updateEntity }
                )
                    updateEntity.SetUpdateOn();
                else if (
                    entry.State == EntityState.Deleted
                    && entry.Entity is ITrackableDelete deleteEntity
                )
                    deleteEntity.SetDeleteOn();
            }
            return ValueTask.FromResult(result);
        }
    }
}
