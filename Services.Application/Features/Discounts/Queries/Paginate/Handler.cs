using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Discounts.Queries.Paginate
{
    public sealed record PaginateDiscountsHandler(IDiscountRepository discountRepository)
        : IRequestHandler<PaginateDiscountsQuery, ResponseOf<PaginateDiscountsResults>>
    {
        public async Task<ResponseOf<PaginateDiscountsResults>> Handle(
            PaginateDiscountsQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                int page = request.page == 0 ? 1 : request.page;
                int pagesize = request.pageSize == 0 ? 10 : request.pageSize;
                IReadOnlyCollection<DiscountsResult> result =
                    await discountRepository.PaginateAsync(
                        page,
                        pagesize,
                        s => new DiscountsResult(s.Id, s.Percentage, s.CreateOn),
                        s => s.Id == request.Id,
                        null!,
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
