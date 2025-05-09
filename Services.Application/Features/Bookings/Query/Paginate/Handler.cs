using System.Linq.Expressions;
using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Shared.Context;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Query.Paginate
{
    public sealed record PaginateBookingsHandler(
        IBookingRepository bookingRepository,
        IUserContext userContext
    ) : IRequestHandler<PaginateBookingsQuery, ResponseOf<PaginateBookingsResults>>
    {
        public async Task<ResponseOf<PaginateBookingsResults>> Handle(
            PaginateBookingsQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                int page = request.page <= 0 ? 1 : request.page;
                int pagesize = request.pageSize <= 0 ? 10 : request.pageSize;

                var userType = userContext.UserType.Value;

                Expression<Func<Booking, BookingsResult>> selector = s => new BookingsResult(
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
                );

                Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>> includes = c =>
                    c.Include(b => b.Customer)
                        .ThenInclude(u => u!.User)
                        .Include(b => b.Worker)
                        .ThenInclude(u => u!.User)
                        .Include(s => s.Service);

                Expression<Func<Booking, object>> orderBy = b => b.CreateOn;

                Expression<Func<Booking, bool>> predicate = userType switch
                {
                    var t when t == UserType.Customer.ToString() => b =>
                        (request.Id != null || b.CustomerId == request.Id),
                    var t when t == UserType.Worker.ToString() => b =>
                        (request.Id != null || b.WorkerId == request.Id),
                    var t when t == UserType.Admin.ToString() => b =>
                        request.Id == null || b.Id == request.Id,
                    _ => throw new InvalidOperationException("Unknown user type."),
                };

                var result = await bookingRepository.PaginateAsync(
                    page,
                    pagesize,
                    selector,
                    predicate,
                    includes,
                    orderBy,
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
                        (int)Math.Ceiling(result.count / (double)pagesize),
                        result.Item1
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
