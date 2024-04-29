using CarWashes.Core.Models;

namespace CarWashes.Core.Interfaces
{
	public interface IUsersService
	{
		Task AddUser(User user);
		Task<string> Login(string login, string password);
	}
}