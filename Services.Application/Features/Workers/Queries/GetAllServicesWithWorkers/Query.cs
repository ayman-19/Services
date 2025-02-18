using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetAllServicesWithWorkers
{
    public sealed record GetAllServicesWithWorkersQuery(Guid WorkerId)
        : IRequest<ResponseOf<GetAllServicesWithWorkersResult>>;
}
