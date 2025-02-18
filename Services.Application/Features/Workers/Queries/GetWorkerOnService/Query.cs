using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetWorkerOnService
{
    public sealed record GetWorkerOnServiceQuery(Guid WorkerId, Guid ServiceId, Guid BranchId)
        : IRequest<ResponseOf<GetWorkerOnServiceResult>>;
}
