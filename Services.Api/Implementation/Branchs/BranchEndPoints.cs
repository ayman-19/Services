using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Abstraction;
using Services.Application.Features.Branchs.Comands.Create;
using Services.Application.Features.Branchs.Comands.Delete;
using Services.Application.Features.Branchs.Comands.Update;
using Services.Application.Features.Branchs.Queries.GetAll;
using Services.Application.Features.Branchs.Queries.GetById;
using Services.Application.Features.Branchs.Queries.Paginate;

namespace Services.Api.Implementation.Branchs
{
    public sealed class BranchEndPoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Branchs").WithTags("Branches");
            group.MapPost(
                "CreateAsync/",
                async (
                    [FromBody] CreateBranchCommand Command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(Command, cancellationToken))
            );

            group.MapPut(
                "UpdateAsync/",
                async (
                    [FromBody] UpdateBranchCommand Command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(Command, cancellationToken))
            );

            group.MapDelete(
                "DeleteAsync/{id}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(await sender.Send(new DeleteBranchCommand(id), cancellationToken))
            );

            group.MapGet(
                "GetByIdAsync/{id}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(await sender.Send(new GetBranchQuery(id), cancellationToken))
            );

            group.MapPost(
                "PaginateAsync",
                async (
                    PaginateBranchQuery query,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(query, cancellationToken))
            );

            group.MapGet(
                "GetAllAsync",
                async (ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(await sender.Send(new GetAllBranchsQuery(), cancellationToken))
            );
        }
    }
}
