using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface IHumansRepository
	{
		Task Add(Human human);
		Task<Result> AddWithUser(Human human, User user);
		Task<List<Human>> GetAll();
		Task<Human> GetByUserId(int id);
		Task<Result<Human>> GetByPhone(string phone);
		Task Update(int id, string phone, string email);
	}
}