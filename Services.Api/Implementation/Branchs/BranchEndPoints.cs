using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Abstraction;
using Services.Application.Features.Branchs.Comands.Create;
using Services.Application.Features.Branchs.Comands.Delete;
using Services.Application.Features.Branchs.Comands.Update;
using Services.Application.Features.Branchs.Queries.GetAll;
using Services.Application.Features.Branchs.Queries.GetById;
using Services.Application.Features.Branchs.Queries.Paginate;
using Services.Shared.Enums;

namespace Services.Api.Implementation.Branchs
{
    public sealed class BranchEndPoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Branchs").WithTags("Locations");
            group
                .MapPost(
                    "CreateAsync/",
                    async (
                        [FromBody] CreateBranchCommand Command,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(Command, cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.CreateBranch));

            group
                .MapPut(
                    "UpdateAsync/",
                    async (
                        [FromBody] UpdateBranchCommand Command,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(Command, cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.UpdateBranch));

            group
                .MapDelete(
                    "DeleteAsync/{id}",
                    async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                        Results.Ok(
                            await sender.Send(new DeleteBranchCommand(id), cancellationToken)
                        )
                )
                .RequireAuthorization(nameof(Permissions.DeleteBranch));

            group
                .MapGet(
                    "GetByIdAsync/{id}",
                    async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                        Results.Ok(await sender.Send(new GetBranchQuery(id), cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.GetBranch));

            group
                .MapPost(
                    "PaginateAsync",
                    async (
                        PaginateBranchQuery query,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(query, cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.PaginateBranch));

            group
                .MapGet(
                    "GetAllAsync",
                    async (ISender sender, CancellationToken cancellationToken) =>
                        Results.Ok(await sender.Send(new GetAllBranchsQuery(), cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.GetAllBranchs));
        }
    }
}
