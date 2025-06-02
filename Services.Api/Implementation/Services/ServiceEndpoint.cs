using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Abstraction;
using Services.Application.Features.Services.Commands.Create;
using Services.Application.Features.Services.Commands.Delete;
using Services.Application.Features.Services.Commands.Update;
using Services.Application.Features.Services.Queries.GetAll;
using Services.Application.Features.Services.Queries.GetById;
using Services.Application.Features.Services.Queries.Paginate;
using Services.Shared.Enums;

namespace Services.Api.Implementation.Services
{
    public class ServiceEndpoint : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Services").WithTags("Services");
            group
                .MapPost(
                    "CreateAsync/",
                    async (
                        [FromForm] CreateServiceCommand Command,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(Command, cancellationToken))
                )
                .DisableAntiforgery()
                .RequireAuthorization(nameof(Permissions.CreateService));

            group
                .MapPut(
                    "UpdateAsync/",
                    async (
                        [FromBody] UpdateServiceCommand Command,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(Command, cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.UpdateService));

            group
                .MapDelete(
                    "DeleteAsync/{id}",
                    async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                        Results.Ok(
                            await sender.Send(new DeleteServiceCommand(id), cancellationToken)
                        )
                )
                .RequireAuthorization(nameof(Permissions.DeleteService));

            group
                .MapGet(
                    "GetByIdAsync/{id}",
                    async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                        Results.Ok(await sender.Send(new GetServiceQuery(id), cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.GetService));

            group
                .MapPost(
                    "PaginateAsync",
                    async (
                        PaginateServiceQuery query,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(query, cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.PaginateService));
            group.MapGet(
                "GetAllAsync/{categoryId}",
                async (Guid categoryId, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(
                        await sender.Send(new GetAllServicesQuery(categoryId), cancellationToken)
                    )
            );
            //.RequireAuthorization(nameof(Permissions.GetAllServices));
        }
    }
}
