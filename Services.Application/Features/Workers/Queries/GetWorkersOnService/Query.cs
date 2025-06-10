using MediatR;
using Services.Domain.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetWorkersOnService
{
    public sealed record GetWorkersOnServiceQuery(
        Guid ServiceId,
        string? searchName,
        double Latitude,
        double Longitude,
        Status? Status
    ) : IRequest<ResponseOf<GetWorkersOnServiceResult>>;
}
