using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Services.Queries.Paginate
{
    public sealed record PaginateServiceQuery(int page, int pageSize)
        : IRequest<ResponseOf<IReadOnlyCollection<PaginateServiceResult>>>;
}
