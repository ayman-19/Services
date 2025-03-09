using MediatR;
using Services.Api.Abstraction;
using Services.Application.Features.Categories.Queries.PaginateParentCategories;
using Services.Application.Features.Categories.Queries.PaginateSubCategoriesByParentCategory;

namespace Services.Api.Implementation.Categories
{
    public class CategoryEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Categories").WithTags("Categories");
            group.MapGet(
                "PaginateParentCategoriesQueryAsync/{page}/{pageSize}",
                async (
                    int page,
                    int pageSize,
                    ISender sender,
                    CancellationToken cancellationToken
                ) =>
                    Results.Ok(
                        await sender.Send(
                            new PaginateParentCategoriesQuery(page, pageSize),
                            cancellationToken
                        )
                    )
            );

            group.MapGet(
                "PaginateSubCategoriesByParentCategoryQueryAsync/{page}/{pageSize}/{parentId}",
                async (
                    int page,
                    int pageSize,
                    Guid parentId,
                    ISender sender,
                    CancellationToken cancellationToken
                ) =>
                    Results.Ok(
                        await sender.Send(
                            new PaginateSubCategoriesByParentCategoryQuery(
                                page,
                                pageSize,
                                parentId
                            ),
                            cancellationToken
                        )
                    )
            );
        }
    }
}
