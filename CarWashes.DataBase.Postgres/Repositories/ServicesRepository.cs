using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using CarWashes.DataBase.Postgres.Models;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace CarWashes.DataBase.Postgres.Repositories
{
	public class ServicesRepository : IServicesRepository
	{
		private readonly CarWashesDbContext _dbContext;
		public ServicesRepository(CarWashesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Service>> GetAll()
		{
			var servicesEntities = await _dbContext.Services
				.AsNoTracking()
				.OrderBy(x => x.Id)
				.ToListAsync();
			var services = servicesEntities
				.Select(x => new Service(
					x.Id, x.CarwashId,
					x.Name, x.Price, x.Duration))
				.ToList();
			return services;
		}

		public async Task<Result<Service>> GetById(int id)
		{
			var serviceEntity = await _dbContext.Services
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
			if (serviceEntity == null)
			{
				return Result.Failure<Service>("Услуга не найдена");
			}
			var service = new Service(
				serviceEntity.Id, serviceEntity.CarwashId,
				serviceEntity.Name, serviceEntity.Price, serviceEntity.Duration);
			return Result.Success(service);
		}

		public async Task<List<Service>> GetByCarwashId(int carwashId)
		{
			var serviceEntities = await _dbContext.Services
				.AsNoTracking()
				.Where(x => x.CarwashId == carwashId)
				.ToListAsync();
			var services = serviceEntities
				.Select(x => new Service(
					x.Id, x.CarwashId,
					x.Name, x.Price, x.Duration))
				.ToList();
			return services;
		}

		public async Task Add(Service service)
		{
			var serviceEntity = new ServiceEntity
			{
				Id = service.Id,
				CarwashId = service.CarwashId,
				Name = service.Name,
				Price = service.Price,
				Duration = service.Duration,
			};

			await _dbContext.Services.AddAsync(serviceEntity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Update(int id, string name, decimal price, TimeSpan duration)
		{
			await _dbContext.Services
				.Where(x => x.Id == id)
				.ExecuteUpdateAsync(s => s
					.SetProperty(x => x.Name, x => name)
					.SetProperty(x => x.Price, x => price)
					.SetProperty(x => x.Duration, x => duration));
		}
	}
}
