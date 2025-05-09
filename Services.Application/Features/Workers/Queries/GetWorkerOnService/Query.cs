using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetWorkerOnService
{
    public sealed record GetWorkerOnServiceQuery(Guid WorkerId)
        : IRequest<ResponseOf<GetWorkerOnServiceResult>>;
}
