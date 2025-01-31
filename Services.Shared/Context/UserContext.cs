using Microsoft.AspNetCore.Http;
using Services.Shared.Enums;

namespace Services.Shared.Context
{
	public sealed class UserContext : IUserContext
	{
		private readonly IHttpContextAccessor _contextAccessor;

		public UserContext(IHttpContextAccessor contextAccessor)
		{
			_contextAccessor = contextAccessor;
		}

		public (string Value, bool Exist) UserId
		{
			get
			{
				if (_contextAccessor.HttpContext != null
					&& _contextAccessor.HttpContext.User.Claims != null
					&& _contextAccessor.HttpContext.User.Claims.Any())
				{

					string userId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(type => type.Type == nameof(RoleClaims.UserId))!.Value;
					if (userId != null)
						return (userId, true);
					else
						return (string.Empty, false);
				}
				else
					return (string.Empty, false);
			}
		}
	}
}
