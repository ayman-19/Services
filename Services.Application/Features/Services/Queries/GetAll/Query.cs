using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Services.Queries.GetAll
{
    public sealed record GetAllServicesQuery(Guid categoryId)
        : IRequest<ResponseOf<IReadOnlyCollection<GetAllServicesResult>>>;
}
