using Services.Domain.Enums;

namespace Services.Application.Features.Bookings.Query.GetById
{
    public sealed record GetBookingByIdResult(
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
