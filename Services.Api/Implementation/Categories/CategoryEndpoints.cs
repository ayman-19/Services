using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Abstraction;
using Services.Application.Features.Branchs.Comands.Create;
using Services.Application.Features.Branchs.Comands.Delete;
using Services.Application.Features.Branchs.Comands.Update;
using Services.Application.Features.Categories.Commands.Create;
using Services.Application.Features.Categories.Commands.Delete;
using Services.Application.Features.Categories.Commands.Update;
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

            group.MapPost(
                "CreateAsync/",
                async (
                    CreateCategoryCommand Command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(Command, cancellationToken))
            );

            group.MapPut(
                "UpdateAsync/",
                async (
                    UpdateCategoryCommand Command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(Command, cancellationToken))
            );

            group.MapDelete(
                "DeleteAsync/{id}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(await sender.Send(new DeleteCategoryCommand(id), cancellationToken))
            );
        }
    }
}
