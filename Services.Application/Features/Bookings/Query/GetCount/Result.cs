using Services.Domain.Enums;

namespace Services.Application.Features.Bookings.Query.GetCount
{
    public sealed record GetCountBookingsResult(int Count, BookingStatus Status);
}
