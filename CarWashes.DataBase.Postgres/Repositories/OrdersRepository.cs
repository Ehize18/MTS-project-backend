using CarWashes.Core;
using CarWashes.Core.Models;
using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWashes.DataBase.Postgres.Repositories
{
	public class OrdersRepository
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

		public async Task Add(Order order)
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
				OrderTime = order.OrderTime,
				CreatedAt = order.CreatedAt,
				UpdatedAt = order.UpdatedAt
			};

			await _dbContext.Orders.AddAsync(orderEntity);
			await _dbContext.SaveChangesAsync();
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
