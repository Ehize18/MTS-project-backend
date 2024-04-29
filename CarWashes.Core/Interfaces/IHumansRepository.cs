using CarWashes.Core.Models;

namespace CarWashes.Core.Interfaces
{
	public interface IHumansRepository
	{
		Task Add(Human human);
		Task AddWithUser(Human human, User user);
		Task<List<Human>> GetAll();
		Task<Human> GetByUserId(int id);
		Task<Human> GetByPhone(string phone);
		Task Update(int id, string phone, string email);
	}
}