using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.DiscountRules.Queries.GetNearestPoints
{
    public sealed record GetNearestPointsQuery(int points)
        : IRequest<ResponseOf<GetNearestPointsResult>>;
}
