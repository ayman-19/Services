using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Query.GetById
{
    public sealed class GetBookingByIdHandler(IBookingRepository bookingRepository)
        : IRequestHandler<GetBookingByIdQuery, ResponseOf<GetBookingByIdResult>>
    {
        public async Task<ResponseOf<GetBookingByIdResult>> Handle(
            GetBookingByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await bookingRepository.GetAsync(
                    s => s.Id == request.Id,
                    s => new GetBookingByIdResult(
                        s.Id,
                        s.CreateOn,
                        s.Status,
                        s.Location,
                        s.CustomerId,
                        s.Customer!.User.Name,
                        s.WorkerId,
                        s.Worker!.User!.Name,
                        s.ServiceId,
                        s.Service.Name,
                        s.Total,
                        s.Rate ?? 0
                    ),
                    c =>
                        c.Include(cust => cust.Customer)
                            .ThenInclude(user => user!.User)
                            .Include(work => work.Worker)
                            .ThenInclude(user => user!.User)
                            .Include(s => s.Service),
                    false,
                    cancellationToken
                );
                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = result,
                };
            }
            catch
            {
                throw new DatabaseTransactionException(ValidationMessages.Database.Error);
            }
        }
    }
}
