using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Abstraction;
using Services.Application.Features.Users.Commands.AddPermissionToRole;
using Services.Application.Features.Users.Commands.Confirm;
using Services.Application.Features.Users.Commands.Create;
using Services.Application.Features.Users.Commands.Delete;
using Services.Application.Features.Users.Commands.ForgetPassword;
using Services.Application.Features.Users.Commands.Login;
using Services.Application.Features.Users.Commands.Logout;
using Services.Application.Features.Users.Commands.ResetPassword;
using Services.Application.Features.Users.Commands.Update;
using Services.Application.Features.Users.Queries.GetById;
using Services.Application.Features.Users.Queries.GetPermissions;
using Services.Application.Features.Users.Queries.GetRoles;
using Services.Shared.Enums;

namespace Services.Api.Implementation.Users
{
    public sealed class UserEndpoint : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/Users").WithTags("Users");

            group.MapPost(
                "/RegisterAsync",
                async (
                    [FromBody] CreateUserCommand command,
                    ISender _sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await _sender.Send(command, cancellationToken))
            );

            group.MapDelete(
                "/DeleteAsync/{id}",
                async (Guid id, ISender _sender, CancellationToken cancellationToken) =>
                    Results.Ok(await _sender.Send(new DeleteUserCommand(id), cancellationToken))
            );

            group.MapPost(
                "/LoginAsync",
                async (
                    LoginUserCommand command,
                    ISender _sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await _sender.Send(command, cancellationToken))
            );

            group
                .MapDelete(
                    "/LogoutAsync",
                    async (ISender _sender, CancellationToken cancellationToken) =>
                        Results.Ok(await _sender.Send(new LogoutUserCommand(), cancellationToken))
                )
                .RequireAuthorization();

            group.MapPut(
                "/UpdateAsync",
                async (
                    [FromBody] UpdateUserCommand command,
                    ISender _sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await _sender.Send(command, cancellationToken))
            );

            group.MapPut(
                "/ConfirmAsync",
                async (
                    [FromBody] ConfirmUserCommand command,
                    ISender _sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await _sender.Send(command, cancellationToken))
            );

            group.MapGet(
                "/GetAsync/{id}",
                async (Guid id, ISender _sender, CancellationToken cancellationToken) =>
                    Results.Ok(await _sender.Send(new GetUserQuery(id), cancellationToken))
            );
            group.MapGet(
                "/GetPermissionsAsync",
                async (ISender _sender, CancellationToken cancellationToken) =>
                    Results.Ok(await _sender.Send(new GetPermissionsQuery(), cancellationToken))
            );
            group.MapGet(
                "/GetRolesAsync",
                async (ISender _sender, CancellationToken cancellationToken) =>
                    Results.Ok(await _sender.Send(new GetRolesQuery(), cancellationToken))
            );

            group.MapGet(
                "/ForgetPasswordAsync/{email}",
                async (string email, ISender _sender, CancellationToken cancellationToken) =>
                    Results.Ok(
                        await _sender.Send(new ForgetPasswordUserCommand(email), cancellationToken)
                    )
            );

            group.MapPut(
                "/ResetPasswordAsync",
                async (
                    ResetPasswordUserCommand command,
                    ISender _sender,
                    CancellationToken cancellationToken
                ) => Results.Ok(await _sender.Send(command, cancellationToken))
            );
            group
                .MapPost(
                    "/AddPermissionToRoleAsync",
                    async (
                        AddPermissionToRoleCommand command,
                        ISender _sender,
                        CancellationToken cancellationToken
                    ) => Results.Ok(await _sender.Send(command, cancellationToken))
                )
                .RequireAuthorization(nameof(Permissions.AddPermissionToRole));
        }
    }
}
