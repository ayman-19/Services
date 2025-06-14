using MediatR;
using Services.Api.Abstraction;
using Services.Application.Features.Workers.Commands.AssignWorkerToService;
using Services.Application.Features.Workers.Commands.RemoveWorkerFromService;
using Services.Application.Features.Workers.Commands.UpdateWorkerOnServiceAvailabilty;
using Services.Application.Features.Workers.Commands.UpdateWorkerStatus;
using Services.Application.Features.Workers.Queries.CalculateRate;
using Services.Application.Features.Workers.Queries.GetAll;
using Services.Application.Features.Workers.Queries.GetAllServicesWithWorkers;
using Services.Application.Features.Workers.Queries.GetWorkerOnService;
using Services.Application.Features.Workers.Queries.GetWorkersBasedOnStatus;
using Services.Application.Features.Workers.Queries.GetWorkersOnService;

namespace Services.Api.Implementation.Workers
{
    public sealed class WorkerEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Workers").WithTags("Workers");

            group
                .MapPost(
                    "AssignWorkerToServiceAsync/",
                    async (
                        AssignWorkerToServiceCommand command,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(command, cancellationToken))
                )
                .RequireAuthorization();
            //.RequireAuthorization(nameof(Permissions.AssignWorkerToService));

            group
                .MapDelete(
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
                )
                .RequireAuthorization();
            //.RequireAuthorization(nameof(Permissions.RemoveWorkerFromService));

            group
                .MapPut(
                    "UpdateWorkerOnServiceAvailabiltyAsync/",
                    async (
                        UpdateWorkerOnServiceAvailabiltyCommand command,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(command, cancellationToken))
                )
                .RequireAuthorization();
            //.RequireAuthorization(nameof(Permissions.UpdateWorkerOnService));
            group
                .MapPut(
                    "UpdateWorkerStatusAsync/",
                    async (
                        UpdateWorkerStatusCommand command,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(command, cancellationToken))
                )
                .RequireAuthorization();
            ///.RequireAuthorization(nameof(Permissions.UpdateWorkerStatus));

            group
                .MapPost(
                    "GetWorkersOnServiceAsync",
                    async (
                        GetWorkersOnServiceQuery query,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(query, cancellationToken))
                )
                .RequireAuthorization();
            //.RequireAuthorization(nameof(Permissions.GetWorkerOnService));

            group
                .MapGet(
                    "GetServiceOnWorkerAsync/{workerId}",
                    async (Guid workerId, ISender sender, CancellationToken cancellationToken) =>
                        Results.Ok(
                            await sender.Send(
                                new GetWorkerOnServiceQuery(workerId),
                                cancellationToken
                            )
                        )
                )
                .RequireAuthorization();
            //.RequireAuthorization(nameof(Permissions.GetWorkerOnService));

            group
                .MapPost(
                    "GetAllServicesOnWorker",
                    async (
                        GetAllServicesWithWorkersQuery query,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(query, cancellationToken))
                )
                .RequireAuthorization();
            //.RequireAuthorization(nameof(Permissions.GetAllServicesWithWorkers));
            group
                .MapPost(
                    "/GetWorkersPaginateAsync",
                    async (
                        GetWorkerPaginateQuery query,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(query, cancellationToken))
                )
                .RequireAuthorization();
            //.RequireAuthorization(nameof(Permissions.GetWorkerPaginate));
            group
                .MapPost(
                    "/GetAllWorkerAsync",
                    async (
                        GetAllWorkerQuery query,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(query, cancellationToken))
                )
                .RequireAuthorization();
            //.RequireAuthorization(nameof(Permissions.GetAllWorker));
            group
                .MapPost(
                    "/GetWorkersBasedOnStatus",
                    async (
                        GetWorkersBasedOnStatusQuery query,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(query, cancellationToken))
                )
                .RequireAuthorization();
            //.RequireAuthorization(nameof(Permissions.GetWorkersBasedOnStatus));
            group
                .MapGet(
                    "/CalculateRateAsync/{workerId}",
                    async (Guid workerId, ISender sender, CancellationToken cancellationToken) =>
                        Results.Ok(
                            await sender.Send(new CalculateRateQuery(workerId), cancellationToken)
                        )
                )
                .RequireAuthorization();
            //.RequireAuthorization(nameof(Permissions.GetWorkersBasedOnStatus));
        }
    }
}
