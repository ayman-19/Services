namespace Services.Application.Features.Workers.Queries.GetWorkersOnService
{
    public sealed record GetWorkersOnServiceResult(
        Guid ServiceId,
        string ServiceName,
        IReadOnlyCollection<GetWorkerResult> WorkerResults
    );

    public sealed record GetWorkerResult(
        Guid WorkerId,
        string WorkerName,
        Guid BranchId,
        string BranchName
    );
}
