using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetWorkerOnService
{
    public sealed record GetWorkerOnServiceQuery(Guid WorkerServiceId)
        : IRequest<ResponseOf<GetWorkerOnServiceResult>>;
}
