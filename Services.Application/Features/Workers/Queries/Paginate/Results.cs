namespace Services.Application.Features.Workers.Queries.Paginate
{
    public sealed record GetAllWorkerPaginateResult(
        Guid WorkerServiceId,
        Guid ServiceId,
        string ServiceName,
        Guid WorkerId,
        string WorkerName,
        Guid BranchId,
        string BranchName,
        bool Availabilty
    );

    public sealed record GetWorkerPaginateResult(
        int Page,
        int PageSize,
        int TotalPage,
        IReadOnlyCollection<GetAllWorkerPaginateResult> workers
    );
}
