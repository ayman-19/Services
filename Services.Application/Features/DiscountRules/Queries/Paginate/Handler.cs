using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.DiscountRules.Queries.Paginate
{
    public sealed record PaginateDiscountRuleHandler(
        IDiscountRuleRepository _discountrulerepository
    ) : IRequestHandler<PaginateDiscountRuleQuery, ResponseOf<PaginateDiscountRuleResult>>
    {
        public async Task<ResponseOf<PaginateDiscountRuleResult>> Handle(
            PaginateDiscountRuleQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                int page = request.page <= 0 ? 1 : request.page;
                int pagesize = request.pageSize <= 0 ? 10 : request.pageSize;
                var result = await _discountrulerepository.PaginateAsync(
                    page,
                    pagesize,
                    b => new DiscountRuleResult(
                        b.Id,
                        b.DiscountId,
                        b.MainPoints,
                        b.Discount.Percentage
                    ),
                    b => b.Id == request.Id || request.Id == null,
                    d => d.Include(d => d.Discount),
                    null!,
                    cancellationToken
                );
                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = new(
                        page,
                        pagesize,
                        (int)Math.Ceiling(result.count / (double)pagesize),
                        result.Item1
                    ),
                };
            }
            catch
            {
                throw new DatabaseTransactionException(ValidationMessages.Database.Error);
            }
        }
    }
}
