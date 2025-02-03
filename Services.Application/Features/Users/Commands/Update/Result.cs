using Services.Domain.Models;

namespace Services.Application.Features.Users.Commands.Update
{
	public sealed record UpdateUserResult(
	Guid Id,
	string Name,
	string Email,
	DateTime CreatedOn,
	IEnumerable<string> roles
)
	{
		public static implicit operator UpdateUserResult(User user) =>
			new(
				user.Id,
				user.Name,
				user.Email,
				user.CreateOn,
				user.UserRoles.Select(ur => ur.Role.Name)
			);
	}
}
