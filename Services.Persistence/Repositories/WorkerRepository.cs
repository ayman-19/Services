using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
    public sealed class WorkerRepository : Repository<Worker>, IWorkerRepository
    {
        public WorkerRepository(ServiceDbContext context)
            : base(context) { }
    }
}
