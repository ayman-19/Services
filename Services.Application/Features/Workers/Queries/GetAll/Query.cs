using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetAll
{
    public sealed record GettAllWorkerQuery()
        : IRequest<ResponseOf<IReadOnlyCollection<GetAllWorkerResult>>>;
}
