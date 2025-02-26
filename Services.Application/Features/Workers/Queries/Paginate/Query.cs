using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.Paginate
{
    public sealed record PaginateWorkerServiceQuery(int page, int pageSize)
        : IRequest<ResponseOf<IReadOnlyCollection<PaginateWorkerServiceResult>>>;
}
