﻿using MediatR;
using Services.Api.Abstraction;
using Services.Application.Features.Services.Queries.GetAll;
using Services.Application.Features.Services.Queries.Paginate;
using Services.Application.Features.Workers.Commands.AssignWorkerToService;
using Services.Application.Features.Workers.Commands.RemoveWorkerFromService;
using Services.Application.Features.Workers.Commands.UpdateWorkerOnServiceAvailabilty;
using Services.Application.Features.Workers.Queries.GetAll;
using Services.Application.Features.Workers.Queries.GetAllServicesWithWorkers;
using Services.Application.Features.Workers.Queries.GetWorkerOnService;
using Services.Application.Features.Workers.Queries.GetWorkersOnService;
using Services.Application.Features.Workers.Queries.Paginate;

namespace Services.Api.Implementation.Workers
{
    public sealed class WorkerEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Workers").WithTags("Workers");

            group.MapPost(
                "AssignWorkerToServiceAsync/",
                async (
                    AssignWorkerToServiceCommand command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(command, cancellationToken))
            );

            group.MapDelete(
                "RemoveWorkerFromService/{workerId}/{serviceId}",
                async (
                    Guid workerId,
                    Guid serviceId,
                    ISender sender,
                    CancellationToken cancellationToken
                ) =>
                    Results.Ok(
                        await sender.Send(
                            new RemoveWorkerFromServiceCommand(workerId, serviceId),
                            cancellationToken
                        )
                    )
            );

            group.MapPut(
                "UpdateWorkerOnServiceAvailabiltyAsync/",
                async (
                    UpdateWorkerOnServiceAvailabiltyCommand command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(command, cancellationToken))
            );

            group.MapGet(
                "GetWorkersOnServiceAsync/{serviceId}",
                async (Guid serviceId, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(
                        await sender.Send(
                            new GetWorkersOnServiceQuery(serviceId),
                            cancellationToken
                        )
                    )
            );

            group.MapGet(
                "GetWorkerOnServiceAsync/{workerId}/{serviceId}/{branchId}",
                async (
                    Guid workerId,
                    Guid serviceId,
                    Guid branchId,
                    ISender sender,
                    CancellationToken cancellationToken
                ) =>
                    Results.Ok(
                        await sender.Send(
                            new GetWorkerOnServiceQuery(workerId, serviceId, branchId),
                            cancellationToken
                        )
                    )
            );
            group.MapGet(
                "GetAllServicesWithWorkers/{workerId}",
                async (Guid workerId, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(
                        await sender.Send(
                            new GetAllServicesWithWorkersQuery(workerId),
                            cancellationToken
                        )
                    )
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
                            new PaginateWorkerServiceQuery(page, pageSize),
                            cancellationToken
                        )
                    )
            );
            group.MapGet(
                "GetAllAsync",
                async (ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(await sender.Send(new GetAllWorkerQuery(), cancellationToken))
            );
        }
    }
}
