using Services.Domain.Entities;
using Services.Domain.Models;

namespace Services.Domain.Abstraction
{
    public interface IJWTManager
    {
		Task<Token> GenerateTokenAsync(User user, CancellationToken cancellationToken = default);
	}
}
