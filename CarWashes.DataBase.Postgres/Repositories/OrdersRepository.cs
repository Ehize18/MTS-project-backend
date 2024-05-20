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
				.Include(o => o.Post)
				.OrderBy(x => x.Id)
				.Include(o => o.User)
				.ThenInclude(u => u.Human)
				.ToListAsync();
			var orders = Map(ordersEntities);
			return orders;
		}

		public async Task<Result<Order>> GetById(int id)
		{
			var orderEntity = await _dbContext.Orders
				.AsNoTracking()
				.Include(o => o.Post)
				.Include(o => o.User)
				.ThenInclude(u => u.Human)
				.FirstOrDefaultAsync(x => x.Id == id);
			if (orderEntity == null)
			{
				return Result.Failure<Order>("Заказ не найден");
			}
			var order = new Order(
				(int)orderEntity.Id,
				orderEntity.UserId, orderEntity.User.Human.F_Name, orderEntity.User.Human.Phone,
				orderEntity.Post.CarWashId, orderEntity.PostId, orderEntity.Post.InternalNumber,
				orderEntity.PlateNumber, orderEntity.CarBrand, orderEntity.CarModel, orderEntity.CarReleaseYear,
				orderEntity.Status, orderEntity.OrderTime, orderEntity.CreatedAt, orderEntity.UpdatedAt
				);
			return Result.Success(order);
		}

		public async Task<List<Order>> GetByUserId(int userId)
		{
			var ordersEntities = await _dbContext.Orders
				.AsNoTracking()
				.Include(o => o.Post)
				.Where(x => x.UserId == userId)
				.Include(o => o.User)
				.ThenInclude(u => u.Human)
				.ToListAsync();
			var orders = Map(ordersEntities);
			return orders;
		}

		public async Task<List<Order>> GetByCarwashId(int carwashId)
		{
			var ordersEntities = await _dbContext.Orders
				.AsNoTracking()
				.Include(o => o.Post)
				.Where(o => o.Post.CarWashId == carwashId)
				.Include(o => o.User)
				.ThenInclude(u => u.Human)
				.ToListAsync();
			var orders = Map(ordersEntities);
			return orders;
		}

		public async Task<List<Order>> GetByCarwashIdWithPagination(int carwashId, int page, int pageSize)
		{
			var ordersEntities = await _dbContext.Orders
				.AsNoTracking()
				.Include(o => o.Post)
				.Where(o => o.Post.CarWashId == carwashId)
				.Include(o => o.User)
				.ThenInclude(u => u.Human)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
			var orders = Map(ordersEntities);
			return orders;
		}

		public async Task<Result> Add(int userId, int postId, string plateNumber, string carBrand, string carModel, int carReleaseYear, DateTime orderTime, List<int> servicesIds)
		{
			var time = DateTime.UtcNow;
			var orderEntity = new OrderEntity
			{
				Id = null,
				UserId = userId,
				PostId = postId,
				PlateNumber = plateNumber,
				CarBrand = carBrand,
				CarModel = carModel,
				CarReleaseYear = carReleaseYear,
				Status = Status.Created,
				OrderTime = orderTime.ToUniversalTime(),
				CreatedAt = time,
				UpdatedAt = time
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
				.ThenInclude(x => x.Orders.Where(x => date == DateOnly.FromDateTime(x.OrderTime) && x.Status != Status.Rejected && x.Status != Status.Canceled))
				.FirstOrDefaultAsync();
			if (carwashEntity == null)
			{
				return Result.Failure<List<AvailableTimeForOrder>>("Автомойка не найдена");
			}
			foreach (var post in carwashEntity.Posts)
			{
				var orderTimeStart = date.ToDateTime(TimeOnly.FromDateTime(carwashEntity.WorkTimeStart.UtcDateTime));
				var orderTimeEnd = orderTimeStart.Add(duration);
				var carwashWorkTimeEndDateTime = date.ToDateTime(TimeOnly.FromDateTime(carwashEntity.WorkTimeEnd.UtcDateTime));
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

		public async Task<Result> ChangeStatus(int orderId, Status status)
		{
			var rows = await _dbContext.Orders
				.Where(x => x.Id == orderId &&
								x.Status != Status.Finished &&
								x.Status != Status.Canceled &&
								x.Status != Status.Rejected)
				.ExecuteUpdateAsync(s => s
					.SetProperty(x => x.Status, x => status)
					.SetProperty(x => x.UpdatedAt, x => DateTime.UtcNow));
			if (rows != 1)
			{
				return Result.Failure("Статут заказа изменить не удалось");
			}
			return Result.Success();
		}

		public async Task<Result> ChangeStatusToCanceled(int orderId, string reason)
		{
			var rows = await _dbContext.Orders
				.Where(x => x.Id == orderId && (x.Status == Status.Created || x.Status == Status.Confirmed))
				.ExecuteUpdateAsync(s => s
					.SetProperty(x => x.Status, x => Status.Canceled)
					.SetProperty(x => x.UpdatedAt, x => DateTime.UtcNow));
			if (rows != 1)
			{
				return Result.Failure("Заказ отменить не удалось");
			}
			var canceledOrderEntity = new CanceledOrderEntity
			{
				OrderId = orderId,
				Reason = reason,
			};
			await _dbContext.CanceledOrders.AddAsync(canceledOrderEntity);
			await _dbContext.SaveChangesAsync();
			return Result.Success();
		}

		private static List<Order> Map(List<OrderEntity> ordersEntities)
		{
			return ordersEntities
							.Select(x => new Order(
								(int)x.Id, x.UserId, x.User.Human.F_Name, x.User.Human.Phone, x.Post.CarWashId, x.PostId, x.Post.InternalNumber,
								x.PlateNumber, x.CarBrand, x.CarModel, x.CarReleaseYear,
								x.Status, x.OrderTime, x.CreatedAt, x.UpdatedAt))
							.ToList();
		}
	}
}
