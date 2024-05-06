using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using CarWashes.DataBase.Postgres.Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace CarWashes.DataBase.Postgres.Repositories
{
	public class UsersRepository : IUsersRepository
	{
		private readonly CarWashesDbContext _dbContext;
		public UsersRepository(CarWashesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<User>> GetAll()
		{
			var usersEntities = await _dbContext.Users
				.AsNoTracking()
				.OrderBy(x => x.Id)
				.ToListAsync();
			var users = usersEntities
				.Select(x => new User(x.Id, x.HumanId, x.Role, x.Login, x.Password, x.Vk_token))
				.ToList();
			return users;
		}

		public async Task<Result<User>> GetById(int id)
		{
			var userEntity = await _dbContext.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
			if (userEntity == null)
			{
				return Result.Failure<User>("Пользователь не найден");
			}
			var user = new User(
				userEntity.Id, userEntity.HumanId, userEntity.Role,
				userEntity.Login, userEntity.Password, userEntity.Vk_token);
			return Result.Success<User>(user);
		}

		public async Task<Result<User>> GetAdminByHumanId(int humanId)
		{
			var userEntity = await _dbContext.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => (x.HumanId == humanId) & (x.Role == "admin"));
			if (userEntity == null)
			{
				return Result.Failure<User>("Сотрудник не найден");
			}
			var user = new User(
				userEntity.Id, userEntity.HumanId, userEntity.Role,
				userEntity.Login, userEntity.Password, userEntity.Vk_token);
			return Result.Success<User>(user);
		}

		public async Task<Result<User>> GetByLogin(string login)
		{
			var userEntity = await _dbContext.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Login == login);
			if (userEntity == null)
			{
				return Result.Failure<User>("Пользователь не найден");
			}
			var user = new User(
				userEntity.Id, userEntity.HumanId, userEntity.Role,
				userEntity.Login, userEntity.Password, userEntity.Vk_token);
			return Result.Success<User>(user);
		}

		public async Task<Result> Add(User user)
		{
			var userEntity = new UserEntity
			{
				HumanId = user.HumanId,
				Role = user.Role,
				Login = user.Login,
				Password = user.Password,
				Vk_token = user.VkToken
			};
			try
			{
				await _dbContext.AddAsync(userEntity);
				await _dbContext.SaveChangesAsync();
				return Result.Success();
			}
			catch
			{
				return Result.Failure("Пользователь с такими данными уже существует");
			}
				
		}

		public async Task Update(int id, string role, string login, string password, string vk_token)
		{
			await _dbContext.Users
				.Where(x => x.Id == id)
				.ExecuteUpdateAsync(s => s
					.SetProperty(x => x.Role, x => role)
					.SetProperty(x => x.Login, x => login)
					.SetProperty(x => x.Password, x => password)
					.SetProperty(x => x.Vk_token, x => vk_token));
		}
	}
}
