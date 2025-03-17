using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Branchs.Queries.Paginate
{
    public sealed record PaginateBranchQuery(int page, int pageSize, Guid? Id)
        : IRequest<ResponseOf<IReadOnlyCollection<PaginateBranchResult>>>;
}
