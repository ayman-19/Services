using Services.Domain.Entities;
using Services.Domain.Repositories;

namespace Services.Domain.Abstraction
{
	public interface ITokenRepository : IRepository<Token>
	{
		Task DeleteByUserIdAsync(Guid userId);
	}
}
