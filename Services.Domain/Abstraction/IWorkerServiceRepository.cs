using Services.Domain.Entities;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
    public interface IWorkerServiceRepository : IRepository<WorkerService>
    {
        ValueTask DeleteWorkerFromServiceAsync(
            Guid WorkerId,
            Guid ServiceId,
            CancellationToken cancellationToken
        );

        ValueTask<double> GetRateAsync(Guid workerId, CancellationToken cancellationToken);

        ValueTask<WorkerService> GetWorkerFromServiceAsync(
            Guid WorkerId,
            Guid ServiceId,
            CancellationToken cancellationToken
        );
        Task RateWorkersAsync(Guid WorkerId, Guid ServiceId);
    }
}
