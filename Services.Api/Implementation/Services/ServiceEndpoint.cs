using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Abstraction;
using Services.Application.Features.Services.Commands.Create;
using Services.Application.Features.Services.Commands.Delete;
using Services.Application.Features.Services.Commands.Update;
using Services.Application.Features.Services.Queries.GetAll;
using Services.Application.Features.Services.Queries.GetById;
using Services.Application.Features.Services.Queries.Paginate;

namespace Services.Api.Implementation.Services
{
    public class ServiceEndpoint : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Services").WithTags("Services");
            group.MapPost(
                "CreateAsync/",
                async (
                    [FromBody] CreateServiceCommand Command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(Command, cancellationToken))
            );

            group.MapPut(
                "UpdateAsync/",
                async (
                    [FromBody] UpdateServiceCommand Command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(Command, cancellationToken))
            );

            group.MapDelete(
                "DeleteAsync/{id}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(await sender.Send(new DeleteServiceCommand(id), cancellationToken))
            );

            group.MapGet(
                "GetByIdAsync/{id}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(await sender.Send(new GetServiceQuery(id), cancellationToken))
            );

            group.MapGet(
                "PaginateAsync/{page}/{pageSize}/{categoryId}",
                async (
                    int page,
                    int pageSize,
                    Guid categoryId,
                    ISender sender,
                    CancellationToken cancellationToken
                ) =>
                    Results.Ok(
                        await sender.Send(
                            new PaginateServiceQuery(page, pageSize, categoryId),
                            cancellationToken
                        )
                    )
            );
            group.MapGet(
                "GetAllAsync/{categoryId}",
                async (Guid categoryId, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(
                        await sender.Send(new GetAllServicesQuery(categoryId), cancellationToken)
                    )
            );
        }
    }
}
