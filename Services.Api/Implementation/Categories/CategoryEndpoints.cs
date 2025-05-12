using MediatR;
using Services.Api.Abstraction;
using Services.Application.Features.Categories.Commands.Create;
using Services.Application.Features.Categories.Commands.Delete;
using Services.Application.Features.Categories.Commands.Update;
using Services.Application.Features.Categories.Queries.GetAll;
using Services.Application.Features.Categories.Queries.PaginateParentCategories;
using Services.Shared.Enums;

namespace Services.Api.Implementation.Categories
{
    public class CategoryEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Categories").WithTags("Categories");
            group
                .MapPost(
                    "PaginateCategoriesQueryAsync",
                    async (
                        PaginateCategoriesQuery query,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(query, cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.PaginateCategories));

            group.MapPost(
                "GetAllCategoriesAsync",
                async (
                    GetAllCategoriesQuery query,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(query, cancellationToken))
            );

            group
                .MapPost(
                    "CreateAsync/",
                    async (
                        CreateCategoryCommand Command,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(Command, cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.CreateCategory));

            group
                .MapPut(
                    "UpdateAsync/",
                    async (
                        UpdateCategoryCommand Command,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(Command, cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.UpdateCategory));

            group
                .MapDelete(
                    "DeleteAsync/{id}",
                    async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                        Results.Ok(
                            await sender.Send(new DeleteCategoryCommand(id), cancellationToken)
                        )
                )
                .RequireAuthorization(nameof(Permissions.DeleteCategory));
        }
    }
}
