using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Customers.Queries.Paginate
{
    public sealed record PaginateCustomerHandler(ICustomerRepository customerRepository)
        : IRequestHandler<PaginateCustomersQuery, ResponseOf<PaginateCustomersResult>>
    {
        public async Task<ResponseOf<PaginateCustomersResult>> Handle(
            PaginateCustomersQuery request,
            CancellationToken cancellationToken
        )
        {
            var page = Math.Max(1, request.page);
            var pageSize = Math.Max(1, request.pagesize);

            try
            {
                var (customers, totalCount) = await customerRepository.PaginateAsync(
                    page,
                    pageSize,
                    c => new CustomersResult(
                        c.UserId,
                        c.User.Name,
                        c.User.Email,
                        c.User.Phone,
                        c.User.UserType
                    ),
                    c =>
                        c.User.DeleteOn == null
                        && (request.customerId == null || c.UserId == request.customerId)
                        && (request.searchName == null || c.User.Name.Contains(request.searchName)),
                    c => c.Include(u => u.User),
                    null!,
                    cancellationToken
                );

                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return new ResponseOf<PaginateCustomersResult>
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = new PaginateCustomersResult(page, pageSize, totalPages, customers),
                };
            }
            catch (Exception ex)
            {
                return new ResponseOf<PaginateCustomersResult>
                {
                    Message = $"{ValidationMessages.Failure} - {ex.Message}",
                    Success = false,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Result = null!,
                };
            }
        }
    }
}
