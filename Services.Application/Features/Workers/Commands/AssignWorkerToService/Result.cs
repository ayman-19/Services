using Services.Domain.Entities;

namespace Services.Application.Features.Workers.Commands.AssignWorkerToService
{
    public sealed record AssignWorkerToServiceResult(
        Guid WorkerId,
        string WorkerName,
        Guid ServiceId,
        string ServiceName,
        double Price
    )
    {
        public static implicit operator AssignWorkerToServiceResult(WorkerService workerService) =>
            new AssignWorkerToServiceResult(
                workerService.WorkerId,
                workerService.Worker.User.Name,
                workerService.ServiceId,
                workerService.Service.Name,
                workerService.Price
            );
    }
}
