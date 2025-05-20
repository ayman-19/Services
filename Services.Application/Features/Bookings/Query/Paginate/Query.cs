using MediatR;
using Services.Domain.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Bookings.Query.Paginate
{
    public sealed record PaginateBookingsQuery(
        int page,
        int pageSize,
        Guid? Id,
        DateTime? Date,
        BookingStatus? Status,
        bool IsPaid
    ) : IRequest<ResponseOf<PaginateBookingsResults>>;
}
