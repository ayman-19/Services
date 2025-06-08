using MediatR;
using Services.Application.Features.Branchs.Queries.GetById;
using Services.Shared.Responses;

namespace Services.Application.Features.Branchs.Queries.GetByUserId
{
    public sealed record GetBranchByUserIdQuery(Guid UserId)
        : IRequest<ResponseOf<GetBranchResult>>;
}
