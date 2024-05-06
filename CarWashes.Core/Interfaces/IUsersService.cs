using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface IUsersService
	{
		Task AddUser(User user);
		Task<Result<string>> Login(string login, string password);
	}
}