using Services.Domain.Enums;

namespace Services.Application.Features.Workers.Queries.CalculateRate
{
    public sealed record CalculateRateResult(Guid WorkerId, double Rate);
}
