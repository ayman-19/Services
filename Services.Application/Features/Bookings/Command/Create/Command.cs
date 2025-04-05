using MediatR;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Bookings.Command.Create
{
    public sealed record CreateBookingCommand(LocationType Location, Guid CustomerId, Guid WorkerId)
        : IRequest<ResponseOf<CreateBookingResult>>
    {
        public static implicit operator Booking(CreateBookingCommand bookingCommand) =>
            new()
            {
                Location = bookingCommand.Location,
                CustomerId = bookingCommand.CustomerId,
                WorkerId = bookingCommand.WorkerId,
            };
    }
}
