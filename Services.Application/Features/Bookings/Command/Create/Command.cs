using MediatR;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Bookings.Command.Create
{
    public sealed record CreateBookingCommand(
        LocationType LocationType,
        Guid CustomerId,
        Guid WorkerId,
        Guid ServiceId,
        double Price
    ) : IRequest<ResponseOf<CreateBookingResult>>
    {
        public static implicit operator Booking(CreateBookingCommand bookingCommand) =>
            new()
            {
                Location = bookingCommand.LocationType,
                CustomerId = bookingCommand.CustomerId,
                WorkerId = bookingCommand.WorkerId,
                Price = bookingCommand.Price,
                ServiceId = bookingCommand.ServiceId,
                Status = BookingStatus.Pending,
            };
    }
}
