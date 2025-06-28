using MediatR;
using Services.Api.Abstraction;
using Services.Application.Features.Customers.Queries.Paginate;

namespace Services.Api.Implementation.Customers
{
    public sealed class Endpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Customers").WithTags("Customers");
            group.MapPost(
                "PaginateCustomersAsync",
                async (
                    PaginateCustomersQuery query,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(query, cancellationToken))
            );
        }
    }
}
