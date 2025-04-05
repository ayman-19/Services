using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Bookings.Query.GetById
{
    public sealed record GetBookingByIdQuery(Guid Id) : IRequest<ResponseOf<GetBookingByIdResult>>;
}
