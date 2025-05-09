namespace Services.Application.Features.Workers.Queries.GetWorkerOnService
{
    public sealed record GetWorkerOnServiceResult(
        Guid WorkerId,
        string WorkerName,
        Guid ServiceId,
        string ServiceName,
        Guid BranchId
    );
}
