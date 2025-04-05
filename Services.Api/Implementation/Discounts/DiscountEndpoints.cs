using MediatR;
using Services.Api.Abstraction;
using Services.Application.Features.Discount.Queries.GetById;
using Services.Application.Features.Discount.Queries.Paginate;

namespace Services.Api.Implementation.Discounts
{
    public sealed class DiscountEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Discounts").WithTags("Discounts");
            group.MapGet(
                "GetByIdAsync/{id}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(await sender.Send(new GetDiscountByIdQuery(id), cancellationToken))
            );

            group.MapPost(
                "PaginateAsync",
                async (
                    PaginateDiscountsQuery query,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(query, cancellationToken))
            );
        }
    }
}
