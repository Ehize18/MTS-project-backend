using CarWashes.Core.Models;

namespace CarWashes.Core.Interfaces
{
	public interface IJwtProvider
	{
		string GenerateToken(User user);
		int GetId(string token);
		string GetRole(string token);
	}
}