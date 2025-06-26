using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Bookings.Query.UnRatedBooking
{
    public sealed record PaginateUnRatedBookingQuery(
        int page,
        int pageSize,
        Guid? Id,
        DateTime? Date
    ) : IRequest<ResponseOf<PaginateUnRatedBookingResults>>;
}
