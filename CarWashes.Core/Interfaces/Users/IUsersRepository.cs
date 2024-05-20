using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface IUsersRepository
	{
		Task<Result> Add(User user);
		Task<List<User>> GetAll();
		Task<Result<User>> GetById(int id);
		Task<Result<User>> GetAdminByHumanId(int humanId);
		Task<Result<User>> GetByLogin(string login);
		Task Update(int id, string role, string login, string password, string vk_token);
	}
}