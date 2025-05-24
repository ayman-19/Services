namespace Services.Application.Features.DiscountRules.Queries.GetNearestPoints
{
    public sealed record GetNearestPointsResult(
        Guid Id,
        Guid DiscountId,
        int MainPoints,
        double Percentage
    );
}
