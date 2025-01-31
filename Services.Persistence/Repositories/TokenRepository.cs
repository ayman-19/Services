using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Persistence.Data;

namespace Services.Persistence.Repositories
{
	public sealed class TokenRepository : Repository<Token>, ITokenRepository
	{
		private readonly ServiceDbContext _context;
		public TokenRepository(ServiceDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task DeleteByUserIdAsync(Guid userId)
			=> await _context.Set<Token>().Where(token=> token.UserId == userId).ExecuteDeleteAsync();
	}
}
