using MediatR;
using Services.Domain.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Bookings.Query.GetCount
{
    public sealed record GetCountBookingsQuery(Guid UserId, BookingStatus? Status)
        : IRequest<ResponseOf<GetCountBookingsResult>>;
}
