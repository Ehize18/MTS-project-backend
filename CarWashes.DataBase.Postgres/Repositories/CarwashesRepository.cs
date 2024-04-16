using CarWashes.Core.Models;
using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashes.DataBase.Postgres.Repositories
{
	public class CarwashesRepository
	{
		private readonly CarWashesDbContext _dbContext;
		public CarwashesRepository(CarWashesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Carwash>> GetAll()
		{
			var carwashesEntities = await _dbContext.Carwashes
				.AsNoTracking()
				.OrderBy(x => x.Id)
				.ToListAsync();
			var carwashes = carwashesEntities
				.Select(x => new Carwash(
					x.Id,
					x.OrgName, x.Name,
					x.City, x.Address,
					x.Phone, x.Email,
					x.WorkTimeStart, x.WorkTimeEnd))
				.ToList();
			return carwashes;
		}

		public async Task<Carwash> GetById(int id)
		{
			var carwashEntity = await _dbContext.Carwashes
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
			var carwash = new Carwash(
				carwashEntity.Id,
				carwashEntity.OrgName, carwashEntity.Name,
				carwashEntity.City, carwashEntity.Address,
				carwashEntity.Phone, carwashEntity.Email,
				carwashEntity.WorkTimeStart, carwashEntity.WorkTimeEnd);
			return carwash;
		}

		public async Task Add(Carwash carwash)
		{
			var carwashEntity = new CarwashEntity
			{
				Id = carwash.Id,
				OrgName = carwash.OrgName,
				Name = carwash.Name,
				City = carwash.City,
				Address = carwash.Address,
				Phone = carwash.Phone,
				Email = carwash.Email,
				WorkTimeStart = carwash.WorkTimeStart,
				WorkTimeEnd = carwash.WorkTimeEnd
			};
			await _dbContext.Carwashes.AddAsync(carwashEntity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Update(int id,
			string phone, string email,
			DateTimeOffset workTimeStart, DateTimeOffset workTimeEnd)
		{
			await _dbContext.Carwashes
				.Where(x => x.Id == id)
				.ExecuteUpdateAsync(s => s
					.SetProperty(x => x.Phone, x => phone)
					.SetProperty(x => x.Email, x => email)
					.SetProperty(x => x.WorkTimeStart, x => workTimeStart)
					.SetProperty(x => x.WorkTimeEnd, x => workTimeEnd));
		}
	}
}
