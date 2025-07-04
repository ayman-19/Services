﻿using MediatR;
using Services.Domain.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Bookings.Command.Update
{
    public sealed record UpdateBookingCommand(
        Guid Id,
        DateTime CreateOn,
        LocationType Location,
        Guid CustomerId,
        Guid WorkerId,
        Guid ServiceId,
        BookingStatus Status,
        string Description,
        bool IsPaid,
        double Rate,
        double Total
    ) : IRequest<ResponseOf<UpdateBookingResult>>;
}
