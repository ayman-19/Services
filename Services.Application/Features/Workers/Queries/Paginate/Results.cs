using Services.Domain.Enums;

namespace Services.Application.Features.Workers.Queries.Paginate
{
    public sealed record GetAllWorkerPaginateResult(
        Guid WorkerServiceId,
        Guid ServiceId,
        string ServiceName,
        Guid WorkerId,
        string WorkerName,
        Guid BranchId,
        bool Availabilty,
        Status Status
    );

    public sealed record GetWorkerPaginateResult(
        int Page,
        int PageSize,
        int TotalPage,
        IReadOnlyCollection<GetAllWorkerPaginateResult> workers
    );
}
