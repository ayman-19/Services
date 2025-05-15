using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Customers.Queries.Paginate
{
    public sealed record PaginateCustomersQuery(
        Guid? customerId,
        string searchName,
        int page,
        int pagesize
    ) : IRequest<ResponseOf<PaginateCustomersResult>>;
}
