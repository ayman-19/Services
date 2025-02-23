using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Services.Queries.GetAll
{
    public sealed record GetAllServicesQuery()
        : IRequest<ResponseOf<IReadOnlyCollection<GetAllServicesResult>>>;
}
