using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.DiscountRules.Queries.GetById
{
    public sealed record GetDiscountRulesHandler(IDiscountRuleRepository _discountruleRepository)
        : IRequestHandler<GetDiscountRuleQuery, ResponseOf<GetDiscountRuleResult>>
    {
        public async Task<ResponseOf<GetDiscountRuleResult>> Handle(
            GetDiscountRuleQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await _discountruleRepository.GetAsync(
                    b => b.Id == request.Id,
                    b => new GetDiscountRuleResult(
                        b.Id,
                        b.DiscountId,
                        b.MainPoints,
                        b.Discount.Percentage
                    ),
                    b => b.Include(d => d.Discount),
                    false,
                    cancellationToken
                );
                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = result,
                };
            }
            catch
            {
                throw new DatabaseTransactionException(ValidationMessages.Database.Error);
            }
        }
    }
}
