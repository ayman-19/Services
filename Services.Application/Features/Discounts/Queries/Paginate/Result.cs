namespace Services.Application.Features.Discounts.Queries.Paginate
{
    public sealed record PaginateDiscountsResults(
        int Page,
        int PageSize,
        int TotalPage,
        IReadOnlyCollection<DiscountsResult> Discounts
    );

    public sealed record DiscountsResult(Guid Id, double Percentage, DateTime ExpireOn);
}
