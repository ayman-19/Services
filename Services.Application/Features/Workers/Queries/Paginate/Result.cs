namespace Services.Application.Features.Workers.Queries.Paginate
{
    public sealed record PaginateWorkerServiceResult(
        Guid WorkerServiceId,
        Guid ServiceId,
        string ServiceName,
        Guid WorkerId,
        string WorkerName,
        Guid BranchId,
        string BranchName,
        bool Availabilty
    );
}
