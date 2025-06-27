namespace Services.Application.Features.Workers.Queries.GetWorkersOnService
{
    public sealed record GetWorkersOnServiceResult(
        int Page,
        int PageSize,
        int TotalPage,
        IQueryable<GetWorkerResult> WorkerResults
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
