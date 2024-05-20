using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface IUsersService
	{
		Task<Result> AddUser(User user);
		Task<Result<string>> Login(string login, string password);
		Task<Result<User>> GetUserById(int id);
		Task<Result<User>> GetAdminByHumanId(int id);
	}
}