using Services.Domain.Enums;

namespace Services.Application.Features.Bookings.Query.UnRatedBooking
{
    public sealed record PaginateUnRatedBookingResults(
        int Page,
        int PageSize,
        int TotalPage,
        IReadOnlyCollection<UnRatedBookingResult> Bookings
    );

    public sealed record UnRatedBookingResult(
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
