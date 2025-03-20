using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class WorkerRepository : Repository<Worker>, IWorkerRepository
    {
        private readonly ServiceDbContext _context;

        public WorkerRepository(ServiceDbContext context)
            : base(context) => _context = context;

        public async Task UpdateStatusAsync(Guid Id, Status status) =>
            await _context
                .Set<Worker>()
                .Where(w => w.UserId == Id)
                .ExecuteUpdateAsync(p => p.SetProperty(p => p.Status, status));
    }
}
