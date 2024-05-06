using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface IHumansService
	{
		Task AddUser(Human human);
		Task<Result> AddHumanWithUser(Human human, User user);
		Task<Human> GetHumanById(int id);
		Task<Result<Human>> GetHumanByPhone(string phone);
	}
}