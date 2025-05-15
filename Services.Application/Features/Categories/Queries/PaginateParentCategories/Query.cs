using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Categories.Queries.PaginateParentCategories
{
    public sealed record PaginateCategoriesQuery(
        int page,
        int pageSize,
        Guid? Id,
        string searchName
    ) : IRequest<ResponseOf<PaginateCategoriesResults>>;
}
