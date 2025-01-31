using MediatR;
using Services.Api.Abstraction;
using Services.Application.Features.Users.Commands.Create;
using Services.Application.Features.Users.Commands.Delete;

namespace Services.Api.Implementation.Users
{
	public class UserEndpoint : IEndpoint
	{
		public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
		{
			RouteGroupBuilder group = endpoints.MapGroup("/Users").WithTags("Users");

			group.MapPost("/CreateAsync", async (CreateUserCommand command, ISender _sender,CancellationToken cancellationToken) =>
			 await _sender.Send(command, cancellationToken));

			group.MapDelete("/DeleteAsync", async (DeleteUserCommand command, ISender _sender,CancellationToken cancellationToken) =>
			 await _sender.Send(command, cancellationToken));

			throw new NotImplementedException();
		}
	}
}
