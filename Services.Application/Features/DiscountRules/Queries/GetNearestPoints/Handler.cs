using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.DiscountRules.Queries.GetNearestPoints
{
    public sealed record GetNearestPointsHandler(IDiscountRuleRepository _discountruleRepository)
        : IRequestHandler<GetNearestPointsQuery, ResponseOf<GetNearestPointsResult>>
    {
        public async Task<ResponseOf<GetNearestPointsResult>> Handle(
            GetNearestPointsQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await _discountruleRepository.GetNearestPointsAsync(
                    request.points,
                    b => new GetNearestPointsResult(
                        b.Id,
                        b.DiscountId,
                        b.MainPoints,
                        b.Discount.Percentage
                    ),
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
