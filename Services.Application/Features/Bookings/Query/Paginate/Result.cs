using Services.Domain.Enums;

namespace Services.Application.Features.Bookings.Query.Paginate
{
    public sealed record PaginateBookingsResults(
        int Page,
        int PageSize,
        int TotalPage,
        IReadOnlyCollection<BookingsResult> Bookings
    );

    public sealed record BookingsResult(
        Guid Id,
        DateTime CreateOn,
        BookingStatus Status,
        LocationType Location,
        Guid CustomerId,
        string CustomerName,
        Guid WorkerId,
        string WorkerName,
        Guid ServiceId,
        string ServiceName,
        string Description,
        bool Ispaid,
        double OldTotal,
        double UpdatedTotal,
        double Rate
    );
}
