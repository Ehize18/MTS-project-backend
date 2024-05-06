using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface IHumansService
	{
		Task AddUser(Human human);
		Task AddHumanWithUser(Human human, User user);
		Task<Human> GetHumanByJwtToken(int id);
		Task<Result<Human>> GetHumanByPhone(string phone);
	}
}