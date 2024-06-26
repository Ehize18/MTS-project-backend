﻿using CarWashes.Core.Interfaces;
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

		public async Task<Result> AddUser(User user)
		{
			return await _usersRepository.Add(user);
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

		public async Task<Result<User>> GetUserById(int id)
		{
			var result = await _usersRepository.GetById(id);
			if (result.IsFailure)
			{
				return Result.Failure<User>("Пользователь не найден");
			}
			var user = result.Value;
			return Result.Success<User>(user);
		}

		public async Task<Result<User>> GetAdminByHumanId(int id)
		{
			var result = await _usersRepository.GetAdminByHumanId(id);
			return result;
		}
	}
}
