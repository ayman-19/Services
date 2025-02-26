using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetAll
{
    public sealed record GetAllWorkerQuery()
        : IRequest<ResponseOf<IReadOnlyCollection<GetAllWorkerResult>>>;
}
