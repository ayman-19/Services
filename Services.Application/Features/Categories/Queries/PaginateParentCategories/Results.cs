namespace Services.Application.Features.Categories.Queries.PaginateParentCategories
{
    public sealed record PaginateCategoriesResults(
        int Page,
        int PageSize,
        int TotalPage,
        IQueryable<CategoriesResult> Categories
    );

    public sealed record CategoriesResult(
        Guid Id,
        string Name,
        Guid? ParentId,
        IQueryable<CategoriesResult> SubCategories
    );

    public sealed record CategoriesFliter(Guid Id, string Name, Guid? ParentId);
}
