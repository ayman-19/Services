using Services.Domain.Enums;

namespace Services.Application.Features.Customers.Queries.Paginate
{
    public sealed record CustomersResult(
        Guid Id,
        string Name,
        string Gmail,
        string Phone,
        UserType UserType
    );

    public sealed record PaginateCustomersResult(
        int Page,
        int PageSize,
        int TotalPage,
        IReadOnlyCollection<CustomersResult> Customers
    );
}
