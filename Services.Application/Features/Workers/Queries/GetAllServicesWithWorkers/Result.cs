using Services.Application.Features.Services.Queries.GetById;

namespace Services.Application.Features.Workers.Queries.GetAllServicesWithWorkers
{
    public sealed record GetAllServicesWithWorkersResult(
        Guid WorkerId,
        string WorkerName,
        IReadOnlyCollection<GetServiceResult> Services
    );
}
