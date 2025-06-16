using MediatR;
using Services.Api.Abstraction;
using Services.Application.Features.Customers.Queries.Paginate;
using Services.Application.Features.Supports.Contactus;

namespace Services.Api.Implementation.Supports
{
    public sealed class SupportsEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Supports").WithTags("Supports");
            group.MapPost(
                "ContactusAsync",
                async (
                    ContactusCommand command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(command, cancellationToken))
            );
        }
    }
}
