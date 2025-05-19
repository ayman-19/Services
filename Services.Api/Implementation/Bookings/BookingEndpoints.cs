using MediatR;
using Services.Api.Abstraction;
using Services.Application.Features.Bookings.Command.Create;
using Services.Application.Features.Bookings.Command.Delete;
using Services.Application.Features.Bookings.Command.Update;
using Services.Application.Features.Bookings.Query.GetById;
using Services.Application.Features.Bookings.Query.GetCount;
using Services.Application.Features.Bookings.Query.Paginate;
using Services.Shared.Enums;

namespace Services.Api.Implementation.Bookings
{
    public sealed class BookingEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Bookings").WithTags("Bookings");

            group
                .MapPost(
                    "CreateAsync/",
                    async (
                        CreateBookingCommand Command,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(Command, cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.CreateBooking));

            group
                .MapPut(
                    "UpdateAsync/",
                    async (
                        UpdateBookingCommand Command,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(Command, cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.UpdateBooking));

            group
                .MapDelete(
                    "DeleteAsync/{id}",
                    async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                        Results.Ok(
                            await sender.Send(new DeleteBookingCommand(id), cancellationToken)
                        )
                )
                .RequireAuthorization(nameof(Permissions.DeleteBooking));
            ;

            group
                .MapGet(
                    "GetByIdAsync/{id}",
                    async (Guid id, ISender sender, CancellationToken cancellationToken) =>
                        Results.Ok(
                            await sender.Send(new GetBookingByIdQuery(id), cancellationToken)
                        )
                )
                .RequireAuthorization(nameof(Permissions.GetBookingById));

            group
                .MapPost(
                    "PaginateAsync",
                    async (
                        PaginateBookingsQuery query,
                        ISender sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await sender.Send(query, cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.PaginateBookings));

            //not Permission
            group.MapPost(
                "GetCountAsync",
                async (
                    GetCountBookingsQuery query,
                    ISender sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await sender.Send(query, cancellationToken))
            );
        }
    }
}
