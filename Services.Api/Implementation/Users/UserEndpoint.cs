using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Abstraction;
using Services.Application.Features.Users.Commands.Create;
using Services.Application.Features.Users.Commands.Delete;
using Services.Application.Features.Users.Commands.Login;
using Services.Application.Features.Users.Commands.Logout;
using Services.Application.Features.Users.Commands.Update;
using Services.Application.Features.Users.Queries.GetById;

namespace Services.Api.Implementation.Users
{
	public class UserEndpoint : IEndpoint
	{
		public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
		{
			RouteGroupBuilder group = endpoints.MapGroup("/Users").WithTags("Users");

			group.MapPost("/RegisterAsync", async ([FromBody] CreateUserCommand command, ISender _sender, CancellationToken cancellationToken) =>
			 Results.Ok(await _sender.Send(command, cancellationToken)));

			group.MapDelete("/DeleteAsync", async ([FromBody] DeleteUserCommand command, ISender _sender, CancellationToken cancellationToken) =>
			 Results.Ok(await _sender.Send(command, cancellationToken)));

			group.MapDelete("/LoginAsync", async ([FromBody] LoginUserCommand command, ISender _sender, CancellationToken cancellationToken) =>
			 Results.Ok(await _sender.Send(command, cancellationToken)));

			group.MapDelete("/LogoutAsync", async ([FromBody] LogoutUserCommand command, ISender _sender, CancellationToken cancellationToken) =>
			 Results.Ok(await _sender.Send(command, cancellationToken)));

			group.MapDelete("/UpdateAsync", async ([FromBody] UpdateUserCommand command, ISender _sender, CancellationToken cancellationToken) =>
			 Results.Ok(await _sender.Send(command, cancellationToken)));

			group.MapDelete("/GetAsync", async ([FromBody] GetUserQuery query, ISender _sender, CancellationToken cancellationToken) =>
			 Results.Ok(await _sender.Send(query, cancellationToken)));

		}
	}
}
