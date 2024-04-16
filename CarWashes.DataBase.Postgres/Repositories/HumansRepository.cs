using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashes.DataBase.Postgres.Repositories
{
	public class HumansRepository : IHumansRepository
	{
		private readonly CarWashesDbContext _dbContext;
		public HumansRepository(CarWashesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Human>> GetAll()
		{
			var humanEntities = await _dbContext.Humans
				.AsNoTracking()
				.OrderBy(x => x.Id)
				.ToListAsync();
			var humans = humanEntities
				.Select(x => new Human(x.Id, x.L_Name, x.F_Name, x.M_Name, x.Phone, x.Birthday, x.Email))
				.ToList();
			return humans;
		}

		public async Task<Human> GetById(int id)
		{
			var humanEntity = await _dbContext.Humans
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
			var human = new Human(
				humanEntity.Id,
				humanEntity.L_Name, humanEntity.F_Name, humanEntity.M_Name,
				humanEntity.Phone, humanEntity.Birthday, humanEntity.Email);
			return human;
		}

		public async Task Add(Human human)
		{
			var humanEntity = new HumanEntity
			{
				L_Name = human.LastName,
				F_Name = human.FirstName,
				M_Name = human.MiddleName,
				Birthday = human.Birthday,
				Phone = human.Phone,
				Email = human.Email
			};

			await _dbContext.Humans.AddAsync(humanEntity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Update(int id, string phone, string email)
		{
			await _dbContext.Humans
				.Where(x => x.Id == id)
				.ExecuteUpdateAsync(s => s
					.SetProperty(x => x.Phone, x => phone)
					.SetProperty(x => x.Email, x => email));
		}
	}
}
