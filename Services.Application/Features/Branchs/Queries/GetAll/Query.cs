using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Branchs.Queries.GetAll
{
    public sealed record GetAllBranchsQuery()
        : IRequest<ResponseOf<IReadOnlyCollection<GetAllBranchsResult>>>;
}
