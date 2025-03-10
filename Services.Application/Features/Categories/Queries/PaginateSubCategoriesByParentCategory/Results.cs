namespace Services.Application.Features.Categories.Queries.PaginateSubCategoriesByParentCategory
{
    public sealed record PaginateSubCategoriesByParentCategoryResults(
        int Page,
        int PageSize,
        int TotalPage,
        IReadOnlyCollection<SubCategories> Categories
    );

    public sealed record SubCategories(Guid Id, string Name, int NumberOfSubCategories);
}
