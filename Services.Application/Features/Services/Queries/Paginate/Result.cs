namespace Services.Application.Features.Services.Queries.Paginate
{
    public sealed record PaginateServiceResult(
        int Page,
        int PageSize,
        int TotalPage,
        IReadOnlyCollection<ServiceResult> Discounts
    );

    public sealed record ServiceResult(Guid id, string name, string description, string imageUrl);
}
