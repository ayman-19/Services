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
        double Rate,
        double Price,
        double Distance,
        Guid LocationId,
        double Latitude,
        double Longitude
    );
}
