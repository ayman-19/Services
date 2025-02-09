using Services.Api.Abstraction;

namespace Services.Api.Implementation.Services
{
    public class ServiceEndpoint : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Services").WithTags("Services");
            group.MapGet(
                "Get",
                () =>
                {
                    return Results.Ok("Services");
                }
            );
        }
    }
}
