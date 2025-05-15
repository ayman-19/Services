using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Services.Queries.Paginate
{
    public sealed record PaginateServiceQuery(
        int page,
        int pageSize,
        Guid? Id,
        Guid? categoryId,
        string searchName
    ) : IRequest<ResponseOf<PaginateServiceResult>>;
}
