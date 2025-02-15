using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Services.Persistence.Repositories
{
    public sealed class WorkerServiceRepository
        : Repository<WorkerService>,
            IWorkerServiceRepository
    {
        private readonly ServiceDbContext _context;

        public WorkerServiceRepository(ServiceDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async ValueTask DeleteWorkerFromServiceAsync(
            Guid WorkerId,
            Guid ServiceId,
            CancellationToken cancellationToken
        ) =>
            await _context
                .Set<WorkerService>()
                .Where(ws => ws.WorkerId == WorkerId && ws.ServiceId == ServiceId)
                .ExecuteDeleteAsync(cancellationToken);

        public async ValueTask<WorkerService> GetWorkerFromServiceAsync(
            Guid WorkerId,
            Guid ServiceId,
            Guid BranchId,
            CancellationToken cancellationToken
        ) =>
            await _context
                .Set<WorkerService>()
                .AsTracking()
                .FirstAsync(ws =>
                    ws.WorkerId == WorkerId && ws.ServiceId == ServiceId && ws.BranchId == BranchId
                );
    }
}
