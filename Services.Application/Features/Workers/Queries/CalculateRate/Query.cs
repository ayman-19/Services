using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.CalculateRate
{
    public sealed record CalculateRateQuery(Guid WorkerId)
        : IRequest<ResponseOf<CalculateRateResult>>;
}
