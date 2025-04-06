using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Discounts.Queries.GetById
{
    public sealed record GetDiscountByIdQuery(Guid Id)
        : IRequest<ResponseOf<GetDiscountByIdResult>>;
}
