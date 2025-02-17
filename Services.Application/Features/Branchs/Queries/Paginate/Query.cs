using MediatR;
using Services.Application.Features.Services.Queries.Paginate;
using Services.Shared.Responses;

namespace Services.Application.Features.Branchs.Queries.Paginate
{
    public sealed record PaginateBranchQuery(int page, int pageSize)
        : IRequest<ResponseOf<IReadOnlyCollection<PaginateBranchResult>>>;
}
