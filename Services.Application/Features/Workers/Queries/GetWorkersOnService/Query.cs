using MediatR;
using Services.Domain.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetWorkersOnService
{
    public sealed record GetWorkersOnServiceQuery(
        int page,
        int pagesize,
        Guid ServiceId,
        string? searchName,
        double Latitude,
        double Langitude,
        Status? Status
    ) : IRequest<ResponseOf<GetWorkersOnServiceResult>>;
}
