using MediatR;
using Services.Application.Features.Workers.Queries.Paginate;
using Services.Domain.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetAll
{
    public sealed record GetWorkerPaginateQuery(
        Guid? WorkerId,
        Guid? ServiceId,
        int page,
        int pagesize
    ) : IRequest<ResponseOf<GetWorkerPaginateResult>>;
}
