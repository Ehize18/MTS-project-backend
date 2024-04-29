using CarWashes.Core.Models;

namespace CarWashes.Core.Interfaces
{
	public interface IUsersRepository
	{
		Task Add(User user);
		Task<List<User>> GetAll();
		Task<User?> GetById(int id);
		Task<User?> GetByLogin(string login);
		Task Update(int id, string role, string login, string password, string vk_token);
	}
}