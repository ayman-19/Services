using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.DiscountRules.Queries.Paginate
{
    public sealed record PaginateDiscountRuleQuery(int page, int pageSize, Guid? Id)
        : IRequest<ResponseOf<PaginateDiscountRuleResult>>;
}
