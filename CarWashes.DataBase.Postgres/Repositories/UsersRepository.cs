using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashes.DataBase.Postgres.Repositories
{
	public class UsersRepository
	{
		private readonly CarWashesDbContext _dbContext;
		public UsersRepository(CarWashesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<UserEntity>> Get()
		{
			return await _dbContext.Users
				.AsNoTracking()
				.OrderBy(x => x.Id)
				.ToListAsync();
		}

		public async Task<List<UserEntity>> GetWithOrders()
		{
			return await _dbContext.Users
				.AsNoTracking()
				.Include(x => x.Orders)
				.ToListAsync();
		}

		public async Task<UserEntity?> GetById(int id)
		{
			return await _dbContext.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<List<UserEntity>> GetOnlyWithOrders()
		{
			return await _dbContext.Users
				.AsNoTracking()
				.Where(x => x.Orders.Count > 0)
				.ToListAsync();
		}

		public async Task Add(int human_id, string role, string login, string password, string vk_token)
		{
			var userEntity = new UserEntity
			{
				HumanId = human_id,
				Role = role,
				Login = login,
				Password = password,
				Vk_token = vk_token
			};

			await _dbContext.AddAsync(userEntity);
			await _dbContext.SaveChangesAsync();
		}
	}
}
