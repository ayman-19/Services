using Services.Domain.Enums;

namespace Services.Application.Features.Workers.Queries.GetWorkersBasedOnStatus
{
    public sealed record GetWorkerStatusPaginateResult(
        int Page,
        int PageSize,
        int TotalPage,
        IReadOnlyCollection<GetWorkers> Workers
    );

    public sealed record GetWorkers(Guid WorkerId, string WorkerName, Status status);
}
