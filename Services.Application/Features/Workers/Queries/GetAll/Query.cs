using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetAll
{
    public class GetAllWorkerQuery : IRequest<ResponseOf<IReadOnlyCollection<GetAllWorkerResult>>>;
}
