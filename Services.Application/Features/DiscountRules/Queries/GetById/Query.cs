using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.DiscountRules.Queries.GetById
{
    public sealed record GetDiscountRuleQuery(Guid Id)
        : IRequest<ResponseOf<GetDiscountRuleResult>>;
}
