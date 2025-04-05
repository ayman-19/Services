using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Discount.Queries.GetById
{
    public sealed record GetDiscountByIdQuery(Guid Id)
        : IRequest<ResponseOf<GetDiscountByIdResult>>;
}
