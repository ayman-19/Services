using Services.Domain.Entities;
using Services.Domain.Enums;

namespace Services.Application.Features.Bookings.Command.Update
{
    public sealed record UpdateBookingResult(
        Guid Id,
        DateTime CreateOn,
        LocationType Location,
        Guid CustomerId,
        Guid WorkerId,
        BookingStatus Status,
        double Total
    )
    {
        public static implicit operator UpdateBookingResult(Booking b) =>
            new(b.Id, b.CreateOn, b.Location, b.CustomerId, b.WorkerId, b.Status, b.Total);
    }
}
