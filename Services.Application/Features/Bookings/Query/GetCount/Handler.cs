using System.Linq.Expressions;
using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Shared.Context;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Query.GetCount
{
    public sealed record GetCountBookingsHandler(
        IBookingRepository bookingRepository,
        IUserContext userContext
    ) : IRequestHandler<GetCountBookingsQuery, ResponseOf<GetCountBookingsResult>>
    {
        public async Task<ResponseOf<GetCountBookingsResult>> Handle(
            GetCountBookingsQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var userType = userContext.UserType.Value;

                Expression<Func<Booking, bool>> predicate = userType switch
                {
                    var t when t == UserType.Customer.ToString() => b =>
                        b.CustomerId == request.UserId
                        && (request.Status != null || b.Status == request.Status),
                    var t when t == UserType.Worker.ToString() => b =>
                        b.WorkerId == request.UserId
                        && (request.Status != null || b.Status == request.Status),
                    _ => throw new InvalidOperationException("Unknown user type."),
                };

                var result = await bookingRepository.CountAsync(predicate, cancellationToken);

                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = new(result, request.Status.GetValueOrDefault()),
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
