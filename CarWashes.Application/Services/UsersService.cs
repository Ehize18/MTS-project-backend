using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;

namespace CarWashes.Application.Services
{
	public class UsersService : IUsersService
	{
		private readonly IUsersRepository _usersRepository;
		private readonly IJwtProvider _jwtProvider;

		public UsersService(IUsersRepository usersRepository, IJwtProvider jwtProvider)
		{
			_usersRepository = usersRepository;
			_jwtProvider = jwtProvider;
		}

		public async Task AddUser(User user)
		{
			await _usersRepository.Add(user);
		}

		public async Task<string> Login(string login, string password)
		{
			var user = await _usersRepository.GetByLogin(login);

			var check = user.Password == password;

			if (!check)
			{
				throw new Exception("Fail");
			}

			var token = _jwtProvider.GenerateToken(user);

			return token;
		}
	}
}
