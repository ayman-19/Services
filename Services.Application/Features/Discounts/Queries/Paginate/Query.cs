using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Discounts.Queries.Paginate
{
    public sealed record PaginateDiscountsQuery(int page, int pageSize, Guid? Id)
        : IRequest<ResponseOf<PaginateDiscountsResults>>;
}
