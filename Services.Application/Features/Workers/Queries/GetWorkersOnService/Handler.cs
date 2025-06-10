using System.Linq;
using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.GetWorkersOnService;

public sealed class GetWorkersOnServiceHandler
    : IRequestHandler<GetWorkersOnServiceQuery, ResponseOf<GetWorkersOnServiceResult>>
{
    private readonly IServiceRepository _serviceRepository;

    public GetWorkersOnServiceHandler(IServiceRepository serviceRepository) =>
        _serviceRepository = serviceRepository;

    public async Task<ResponseOf<GetWorkersOnServiceResult>> Handle(
        GetWorkersOnServiceQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var serviceEntity = await _serviceRepository.GetAsync(
                s =>
                    s.Id == request.ServiceId
                    && s.WorkerServices.Any(ws =>
                        request.Status == null
                            ? ws.Worker.Status == Status.Active
                            : ws.Worker.Status == request.Status
                    ),
                s => s,
                include =>
                    include
                        .Include(s => s.WorkerServices)
                        .ThenInclude(ws => ws.Worker)
                        .ThenInclude(w => w.User)
                        .ThenInclude(u => u.Branch),
                false,
                cancellationToken
            );

            if (serviceEntity == null)
            {
                // handle not found if needed
                throw new Exception("Service not found");
            }

            var filteredWorkers = serviceEntity
                .WorkerServices.Where(ws =>
                    (
                        string.IsNullOrWhiteSpace(request.searchName)
                        || ws.Worker.User.Name.Contains(request.searchName)
                    )
                    && (
                        request.Status == null
                            ? ws.Worker.Status == Status.Active
                            : ws.Worker.Status == request.Status
                    )
                )
                .Select(ws =>
                {
                    var branch = ws.Worker.User.Branch;
                    return new GetWorkerResult(
                        ws.WorkerId,
                        ws.Worker.User.Name,
                        ws.Worker.WorkerServices.Average(r => r.Rate),
                        ws.Price,
                        GetDistance(
                            request.Latitude,
                            request.Longitude,
                            branch.Latitude,
                            branch.Langitude
                        ),
                        branch.Id,
                        branch.Latitude,
                        branch.Langitude
                    );
                })
                .OrderBy(w => w.Distance)
                .ThenBy(w => w.Rate)
                .ToList();

            var result = new GetWorkersOnServiceResult(
                serviceEntity.Id,
                serviceEntity.Name,
                filteredWorkers
            );
            return new ResponseOf<GetWorkersOnServiceResult>
            {
                Message = ValidationMessages.Success,
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = result,
            };
        }
        catch
        {
            throw new DatabaseTransactionException(ValidationMessages.Database.Error);
        }
    }

    private double GetDistance(double lat1, double lon1, double lat2, double lon2)
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

    private double DegreesToRadians(double degrees) => degrees * (Math.PI / 180);
}
