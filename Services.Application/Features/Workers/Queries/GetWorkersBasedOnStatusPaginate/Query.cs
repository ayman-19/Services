using MediatR;
using Services.Domain.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetWorkersBasedOnStatus
{
    public sealed record GetWorkersBasedOnStatusQuery(
        Guid? WorkerId,
        Status status,
        int page,
        int pagesize
    ) : IRequest<ResponseOf<GetWorkersBasedOnStatusResult>>;
}
