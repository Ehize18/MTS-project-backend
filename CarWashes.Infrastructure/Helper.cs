using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using CSharpFunctionalExtensions;
using System.Security.Cryptography;
using System.Text;

namespace CarWashes.Infrastructure
{
	public class Helper
	{
		public static String SHA256Hash(String value)
		{
			StringBuilder Sb = new StringBuilder();

			using (SHA256 hash = SHA256.Create())
			{
				Encoding enc = Encoding.UTF8;
				Byte[] result = hash.ComputeHash(enc.GetBytes(value));

				foreach (Byte b in result)
					Sb.Append(b.ToString("x2"));
			}

			return Sb.ToString();
		}

		public static async Task<Result<User>> GetAdminByToken(string token, IJwtProvider jwtProvider, IUsersService usersService)
		{
			if (token == null)
			{
				return Result.Failure<User>("Ошибка авторизации");
			}
			var id = jwtProvider.GetId(token);
			if (jwtProvider.GetRole(token) != "admin")
			{
				Console.WriteLine(jwtProvider.GetRole(token));
				return Result.Failure<User>("Ошибка авторизации");
			}
			var userResult = await usersService.GetUserById(id);
			if (userResult.IsFailure)
			{
				return Result.Failure<User>("Ошибка авторизации");
			}
			return Result.Success<User>(userResult.Value);
		}
	}
}
