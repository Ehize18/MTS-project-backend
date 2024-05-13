using CarWashes.Core;
using CarWashes.Core.Models;
using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using CarWashes.Core.Interfaces;
using CSharpFunctionalExtensions;
namespace CarWashes.DataBase.Postgres.Repositories
{
	public class OrdersRepository : IOrdersRepository
	{
		private readonly CarWashesDbContext _dbContext;
		public OrdersRepository(CarWashesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Order>> GetAll()
		{
			var ordersEntities = await _dbContext.Orders
				.AsNoTracking()
				.OrderBy(x => x.Id)
				.ToListAsync();
			var orders = Map(ordersEntities);
			return orders;
		}

		public async Task<List<Order>> GetByUserId(int userId)
		{
			var ordersEntities = await _dbContext.Orders
				.AsNoTracking()
				.Where(x => x.UserId == userId)
				.ToListAsync();
			var orders = Map(ordersEntities);
			return orders;
		}

		public async Task<List<Order>> GetByCarwashId(int carwashId)
		{
			var postsEntities = await _dbContext.Posts
				.AsNoTracking()
				.Include(x => x.Orders)
				.Where(x => x.CarWashId == carwashId)
				.ToListAsync();
			var ordersEntities = postsEntities
				.SelectMany(x => x.Orders).ToList();
			var orders = Map(ordersEntities);
			return orders;

		}

		public async Task<Result> Add(Order order, List<int> servicesIds)
		{
			var orderEntity = new OrderEntity
			{
				Id = order.Id,
				UserId = order.UserId,
				PostId = order.PostId,
				PlateNumber = order.PlateNumber,
				CarBrand = order.CarBrand,
				CarModel = order.CarModel,
				CarReleaseYear = order.CarReleaseYear,
				Status = order.Status,
				OrderTime = order.OrderTime.ToUniversalTime(),
				CreatedAt = order.CreatedAt,
				UpdatedAt = order.UpdatedAt
			};
			foreach (var serviceId in servicesIds)
			{
				var serviceEntity = await _dbContext.Services.
					AsNoTracking()
					.FirstOrDefaultAsync(x => x.Id == serviceId);
				if (serviceEntity == null)
				{
					return Result.Failure("Одна или несколько услуг не найдены");
				}
				serviceEntity.Orders.Add(orderEntity);
				_dbContext.Services.Attach(serviceEntity);
				_dbContext.Services.Update(serviceEntity);
				orderEntity.Services.Add(serviceEntity);
			}
			try
			{
				await _dbContext.Orders.AddAsync(orderEntity);
				await _dbContext.SaveChangesAsync();
				return Result.Success();
			}
			catch (Exception ex)
			{
				return Result.Failure(ex.ToString());
			}
		}

		public async Task<Result<List<AvailableTimeForOrder>>> GetAvailableTimesForOrder(List<Service> services, DateOnly date, int carwashId)
		{
			var duration = new TimeSpan();
			foreach (var service in services)
			{
				if (service.CarwashId != carwashId)
					return Result.Failure<List<AvailableTimeForOrder>>("Не все услуги принадлежат к данной автомойке");
				duration = duration.Add(service.Duration);
				Console.WriteLine(service.Duration.ToString());
			}
			Console.WriteLine(duration.ToString());
			duration = TimeSpan.FromMinutes(30 * Math.Ceiling(duration.TotalMinutes / 30));
			Console.WriteLine(duration.ToString());
			var times = new List<AvailableTimeForOrder>();
			var carwashEntity = await _dbContext.Carwashes
				.Where(x => x.Id == carwashId)
				.Include(x => x.Posts)
				.ThenInclude(x => x.Orders.Where(x => date == DateOnly.FromDateTime(x.OrderTime)))
				.FirstOrDefaultAsync();
			if (carwashEntity == null)
			{
				return Result.Failure<List<AvailableTimeForOrder>>("Автомойка не найдена");
			}
			foreach (var post in carwashEntity.Posts)
			{
				var orderTimeStart = date.ToDateTime(carwashEntity.WorkTimeStart).ToUniversalTime();
				var orderTimeEnd = orderTimeStart.Add(duration);
				var carwashWorkTimeEndDateTime = date.ToDateTime(carwashEntity.WorkTimeEnd);
				while (orderTimeEnd <= carwashWorkTimeEndDateTime)
				{
					if (!post.Orders.Any(
					x =>
					(x.OrderTime < orderTimeEnd && x.OrderTime.Add(new TimeSpan(x.Services.Sum(r => r.Duration.Ticks))) > orderTimeStart)))
					{

						var startEndTime = new List<DateTime>()
						{
							orderTimeStart, orderTimeEnd
						};
						if (!times.Any(x => x.AvailableTime.SequenceEqual(startEndTime)))
						{
							times.Add(new AvailableTimeForOrder((int)post.Id, startEndTime));
						}
							
					}
					orderTimeStart = orderTimeStart.Add(TimeSpan.FromMinutes(30));
					orderTimeEnd = orderTimeEnd.Add(TimeSpan.FromMinutes(30));
					Console.WriteLine(post.Id);
					Console.WriteLine(orderTimeStart.ToString());
					Console.WriteLine(orderTimeEnd.ToString());
				}
			}
			return Result.Success(times);

		}

		public async Task Update(int orderId, Status status, DateTime updatadAt)
		{
			await _dbContext.Orders
				.Where(x => x.Id == orderId)
				.ExecuteUpdateAsync(s => s
					.SetProperty(x => x.Status, x => status)
					.SetProperty(x => x.UpdatedAt, x => updatadAt));
		}

		private static List<Order> Map(List<OrderEntity> ordersEntities)
		{
			return ordersEntities
							.Select(x => new Order(
								x.Id, x.UserId, x.PostId,
								x.PlateNumber, x.CarBrand, x.CarModel, x.CarReleaseYear,
								x.OrderTime, x.CreatedAt, x.UpdatedAt))
							.ToList();
		}
	}
}
