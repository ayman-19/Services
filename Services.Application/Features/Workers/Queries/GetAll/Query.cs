using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetAll
{
    public sealed record GetAllWorkerQuery(Guid? ServiceId)
        : IRequest<ResponseOf<IReadOnlyCollection<GetAllWorkerResult>>>;
}
