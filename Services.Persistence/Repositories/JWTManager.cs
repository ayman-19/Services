using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Domain.Models;
using Services.Shared.Enums;
using Services.Shared.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Persistence.Repositories
{
	public sealed class JWTManager : IJWTManager
	{
		public Task<Token> GenerateTokenAsync(User user, CancellationToken cancellationToken = default)
		{
			var Claims = GetClaims(user);
			var symetreckey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Secret));
			var signCreadential = new SigningCredentials(symetreckey, SecurityAlgorithms.HmacSha256);
			var jwtSecurityToken = new JwtSecurityToken(issuer: JwtSettings.Issuer, audience: JwtSettings.Audience, claims: Claims, signingCredentials: signCreadential, expires: DateTime.Now.AddMonths((int)JwtSettings.AccessTokenExpireDate));
			return Task.FromResult(Token.Create(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), jwtSecurityToken.ValidTo, user.Id));
		}
		private IReadOnlyCollection<Claim> GetClaims(User user)
		{
			List<Claim> claims = new();

			claims.AddRange(
				user.UserRoles.Select(x => new Claim(nameof(RoleClaims.Roles), x.RoleId.ToString()))
			);
			claims.Add(new Claim(nameof(RoleClaims.UserId), user.Id.ToString()));

			return claims;
		}
	}
}
