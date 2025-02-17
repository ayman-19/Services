using System.Text.RegularExpressions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Abstraction;
using Services.Application.Features.Branchs.Comands.Create;
using Services.Application.Features.Branchs.Comands.Delete;
using Services.Application.Features.Branchs.Comands.Update;
using Services.Application.Features.Branchs.Queries.GetById;
using Services.Application.Features.Branchs.Queries.Paginate;

namespace Services.Api.Implementation.Branchs
{
    public class BranchEndPoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Branch").WithTags("Branches");
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

            group.MapGet(
                "PaginateAsync/{page}/{pageSize}",
                async (
                    int page,
                    int pageSize,
                    ISender sender,
                    CancellationToken cancellationToken
                ) =>
                    Results.Ok(
                        await sender.Send(
                            new PaginateBranchQuery(page, pageSize),
                            cancellationToken
                        )
                    )
            );
        }
    }
}
