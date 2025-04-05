using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Query.Paginate
{
    public sealed record PaginateBookingsHandler(IBookingRepository bookingRepository)
        : IRequestHandler<PaginateBookingsQuery, ResponseOf<PaginateBookingsResults>>
    {
        public async Task<ResponseOf<PaginateBookingsResults>> Handle(
            PaginateBookingsQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                int page = request.page == 0 ? 1 : request.page;
                int pagesize = request.pageSize == 0 ? 10 : request.pageSize;
                IReadOnlyCollection<BookingsResult> result = await bookingRepository.PaginateAsync(
                    page,
                    pagesize,
                    s => new BookingsResult(
                        s.Id,
                        s.CreateOn,
                        s.Status,
                        s.Location,
                        s.CustomerId,
                        s.Customer!.User.Name,
                        s.WorkerId,
                        s.Worker!.User!.Name
                    ),
                    s => s.Id == request.Id,
                    c =>
                        c.Include(cust => cust.Customer)
                            .ThenInclude(user => user!.User)
                            .Include(work => work.Worker)
                            .ThenInclude(user => user!.User),
                    cancellationToken
                );
                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = new(
                        page,
                        pagesize,
                        (int)Math.Ceiling(result.Count() / (double)pagesize),
                        result
                    ),
                };
            }
            catch
            {
                throw new DatabaseTransactionException(ValidationMessages.Database.Error);
            }
        }
    }
}
