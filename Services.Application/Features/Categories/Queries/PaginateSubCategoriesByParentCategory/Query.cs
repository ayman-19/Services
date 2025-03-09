using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Categories.Queries.PaginateSubCategoriesByParentCategory
{
    public sealed record PaginateSubCategoriesByParentCategoryQuery(
        int page,
        int pageSize,
        Guid parentCategory
    ) : IRequest<ResponseOf<PaginateSubCategoriesByParentCategoryResults>>;
}
