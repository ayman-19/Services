using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Discount.Queries.GetById
{
    public sealed class GetDiscountByIdHandler(IDiscountRepository discountRepository)
        : IRequestHandler<GetDiscountByIdQuery, ResponseOf<GetDiscountByIdResult>>
    {
        public async Task<ResponseOf<GetDiscountByIdResult>> Handle(
            GetDiscountByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await discountRepository.GetAsync(
                    s => s.Id == request.Id,
                    s => new GetDiscountByIdResult(s.Id, s.Percentage, s.CreateOn),
                    null!,
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
