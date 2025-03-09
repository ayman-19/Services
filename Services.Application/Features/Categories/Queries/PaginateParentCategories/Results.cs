namespace Services.Application.Features.Categories.Queries.PaginateParentCategories
{
    public sealed record PaginateParentCategoriesResults(
        int Page,
        int PageSize,
        int TotalPage,
        IReadOnlyCollection<ParentCategories> Categories
    );

    public sealed record ParentCategories(Guid Id, string Name);
}
