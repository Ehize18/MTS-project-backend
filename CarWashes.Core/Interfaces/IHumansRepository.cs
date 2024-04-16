using CarWashes.Core.Models;

namespace CarWashes.Core.Interfaces
{
	public interface IHumansRepository
	{
		Task Add(Human human);
		Task<List<Human>> GetAll();
		Task<Human> GetById(int id);
		Task Update(int id, string phone, string email);
	}
}