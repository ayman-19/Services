namespace Services.Application.Features.DiscountRules.Queries.GetById
{
    public sealed record GetDiscountRuleResult(
        Guid Id,
        Guid DiscountId,
        int MainPoints,
        double Percentage
    );
}
