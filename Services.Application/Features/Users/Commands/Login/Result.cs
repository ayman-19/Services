using Services.Domain.Models;

namespace Services.Application.Features.Users.Commands.Login
{
	public sealed record LoginUserResult(Guid userId, string content)
	{
		public static implicit operator LoginUserResult(User user) => new(user.Id, user.Token.Content);
	}
}
