using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Enums;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.GetWorkersOnService;

public sealed class GetWorkersOnServiceHandler(IWorkerServiceRepository workerServiceRepository)
    : IRequestHandler<GetWorkersOnServiceQuery, ResponseOf<GetWorkersOnServiceResult>>
{
    public async Task<ResponseOf<GetWorkersOnServiceResult>> Handle(
        GetWorkersOnServiceQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            int page = request.page <= 0 ? 1 : request.page;
            int pagesize = request.pagesize <= 0 ? 10 : request.pagesize;
            var serviceEntity = await workerServiceRepository.PaginateAsync(
                page,
                pagesize,
                ws => new GetWorkerResult(
                    ws.WorkerId,
                    ws.Worker.User.Name,
                    ws.Worker.WorkerServices.Average(r => r.Rate),
                    ws.Price,
                    GetDistance(
                        request.Latitude,
                        request.Langitude,
                        ws.Worker.User.Branch.Latitude,
                        ws.Worker.User.Branch.Langitude
                    ),
                    ws.Worker.User.Branch.Id,
                    ws.Worker.User.Branch.Latitude,
                    ws.Worker.User.Branch.Langitude,
                    ws.Worker.User.Phone
                ),
                ws =>
                    ws.ServiceId == request.ServiceId
                    && ws.Availabilty == true
                    && (
                        request.Status == null
                            ? ws.Worker.Status == Status.Active
                            : ws.Worker.Status == request.Status
                    ),
                include =>
                    include
                        .Include(ws => ws.Worker)
                        .ThenInclude(w => w.User)
                        .ThenInclude(u => u.Branch),
                null!,
                cancellationToken
            );
            IQueryable<GetWorkerResult>? orderedDate = serviceEntity
                .Item1.OrderBy(d => d.Distance)
                .ThenByDescending(r => r.Rate)
                .AsQueryable();

            var result = new GetWorkersOnServiceResult(
                page,
                pagesize,
                serviceEntity.count,
                orderedDate
            );
            return new ResponseOf<GetWorkersOnServiceResult>
            {
                Message = ValidationMessages.Success,
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = result,
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    private static double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double EarthRadiusKm = 6371;

        double dLat = DegreesToRadians(lat2 - lat1);
        double dLon = DegreesToRadians(lon2 - lon1);
        double a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
            + Math.Cos(DegreesToRadians(lat1))
                * Math.Cos(DegreesToRadians(lat2))
                * Math.Sin(dLon / 2)
                * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return EarthRadiusKm * c * 1000;
    }

    private static double DegreesToRadians(double degrees) => degrees * (Math.PI / 180);
}
