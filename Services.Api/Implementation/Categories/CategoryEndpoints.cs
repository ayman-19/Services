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
            group.MapPost(
                "PaginateParentCategoriesQueryAsync",
                async (
                    PaginateParentCategoriesQuery query,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(query, cancellationToken))
            );

            group.MapPost(
                "PaginateSubCategoriesByParentCategoryQueryAsync",
                async (
                    PaginateSubCategoriesByParentCategoryQuery query,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(query, cancellationToken))
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
