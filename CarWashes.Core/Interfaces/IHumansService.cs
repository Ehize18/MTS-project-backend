using CarWashes.Core.Models;

namespace CarWashes.Core.Interfaces
{
	public interface IHumansService
	{
		Task AddUser(Human human);
		Task AddHumanWithUser(Human human, User user);
		Task<Human> GetHumanByJwtToken(int id);
		Task<Human> GetHumanByPhone(string phone);
	}
}