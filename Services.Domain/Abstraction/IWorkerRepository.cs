using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface IWorkerRepository : IRepository<Worker>
    {
        Task UpdateStatusAsync(Guid Id, Status status);
    }
}
