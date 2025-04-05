using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Bookings.Query.Paginate
{
    public sealed record PaginateBookingsQuery(int page, int pageSize, Guid? Id)
        : IRequest<ResponseOf<PaginateBookingsResults>>;
}
