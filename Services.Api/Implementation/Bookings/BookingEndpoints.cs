using MediatR;
using Services.Api.Abstraction;
using Services.Application.Features.Bookings.Command.Create;
using Services.Application.Features.Bookings.Command.Delete;

namespace Services.Api.Implementation.Bookings
{
    public sealed class BookingEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Bookings").WithTags("Bookings");

            group.MapPost(
                "CreateAsync/",
                async (
                    CreateBookingCommand Command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(Command, cancellationToken))
            );

            //group.MapPut(
            //    "UpdateAsync/",
            //    async (
            //        UpdateServiceCommand Command,
            //        ISender sender,
            //        CancellationToken cancellationToken
            //    ) => Results.Ok(await sender.Send(Command, cancellationToken))
            //);

            group.MapDelete(
                "DeleteAsync/{id}",
                async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                    Results.Ok(await sender.Send(new DeleteBookingCommand(id), cancellationToken))
            );
        }
    }
}
