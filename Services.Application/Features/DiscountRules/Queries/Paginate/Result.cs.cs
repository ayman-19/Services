namespace Services.Application.Features.DiscountRules.Queries.Paginate
{
    public sealed record DiscountRuleResult(
        Guid Id,
        Guid DiscountId,
        int MainPoints,
        double Percentage
    );

    public sealed record PaginateDiscountRuleResult(
        int Page,
        int PageSize,
        int TotalPage,
        IReadOnlyCollection<DiscountRuleResult> DiscountRules
    );
}
