using CarWashes.Core.Models;
using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using CarWashes.Core.Interfaces;

namespace CarWashes.DataBase.Postgres.Repositories
{
	public class CarwashesRepository : ICarwashesRepository
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

		public async Task Add(Carwash carwash, User user)
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
			var userEntity = await _dbContext.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == user.Id);
			userEntity.Carwashes.Add(carwashEntity);
			_dbContext.Users.Attach(userEntity);
			_dbContext.Users.Update(userEntity);
			carwashEntity.Staff.Add(userEntity);
			await _dbContext.Carwashes.AddAsync(carwashEntity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Update(int id,
			string phone, string email,
			TimeOnly workTimeStart, TimeOnly workTimeEnd)
		{
			await _dbContext.Carwashes
				.Where(x => x.Id == id)
				.ExecuteUpdateAsync(s => s
					.SetProperty(x => x.Phone, x => phone)
					.SetProperty(x => x.Email, x => email)
					.SetProperty(x => x.WorkTimeStart, x => workTimeStart)
					.SetProperty(x => x.WorkTimeEnd, x => workTimeEnd));
		}

		public async Task AddStaff(Carwash carwash, User user)
		{
			var carwashEntity = await _dbContext.Carwashes
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == carwash.Id);
			var userEntity = await _dbContext.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == user.Id);

			carwashEntity.Staff.Add(userEntity);
			_dbContext.Carwashes.Update(carwashEntity);
			await _dbContext.SaveChangesAsync();
		}
	}
}
