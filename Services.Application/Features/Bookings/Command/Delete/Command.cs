using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Bookings.Command.Delete
{
    public sealed record DeleteBookingCommand(Guid Id) : IRequest<Response>;
}
