using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Categories.Queries.PaginateParentCategories
{
    public sealed record PaginateParentCategoriesQuery(int page, int pageSize, Guid? Id)
        : IRequest<ResponseOf<PaginateParentCategoriesResults>>;
}
