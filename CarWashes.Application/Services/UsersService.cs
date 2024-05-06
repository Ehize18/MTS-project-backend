using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

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

		public async Task<Result<string>> Login(string login, string password)
		{
			var result = await _usersRepository.GetByLogin(login);
			if (result.IsFailure)
			{
				return Result.Failure<string>("Не удалось войти");
			}
			var user = result.Value;

			var check = user.Password == password;

			if (!check)
			{
				return Result.Failure<string>("Не удалось войти");
			}

			var token = _jwtProvider.GenerateToken(user);

			return Result.Success<string>(token);
		}
	}
}
