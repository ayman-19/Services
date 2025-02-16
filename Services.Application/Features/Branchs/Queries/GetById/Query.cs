using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Branchs.Queries.GetById
{
    public sealed record GetBranchQuery(Guid Id) : IRequest<ResponseOf<GetBranchResult>>;
}
