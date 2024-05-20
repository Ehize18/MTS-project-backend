using CarWashes.Core;
using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Application.Services
{
	public class OrdersService : IOrdersService
	{
		private readonly IOrdersRepository _ordersRepository;
		private readonly IServicesRepository _servicesRepository;
		private readonly ICarwashesRepository _carwashesRepository;

		public OrdersService(IOrdersRepository ordersRepository, IServicesRepository servicesRepository, ICarwashesRepository carwashesRepository)
		{
			_ordersRepository = ordersRepository;
			_servicesRepository = servicesRepository;
			_carwashesRepository = carwashesRepository;
		}

		public async Task<Result> AddOrder(int userId, int postId, string plateNumber, string carBrand, string carModel, int CarReleaseYear, DateTime orderTime, List<int> servicesIds)
		{
			var orderResult = await _ordersRepository.Add(userId, postId, plateNumber, carBrand, carModel, CarReleaseYear, orderTime, servicesIds);
			return orderResult;
		}

		public async Task<Result<List<AvailableTimeForOrder>>> GetAvailableTimesForOrder(List<int> servicesIds, DateOnly date, int carwashId)
		{
			var services = new List<Service>();
			foreach (var serviceId in servicesIds)
			{
				var serviceResult = await _servicesRepository.GetById(serviceId);
				if (serviceResult.IsFailure)
					return Result.Failure<List<AvailableTimeForOrder>>("Одна из услуг не найдена");
				services.Add(serviceResult.Value);
			}
			var timesResult = await _ordersRepository.GetAvailableTimesForOrder(services, date, carwashId);
			return timesResult;
		}

		public async Task<Result<List<Order>>> GetOrdersOnCarwashWithPagination(User admin, int carwashId, int page, int pageSize)
		{
			var staff = await _carwashesRepository.GetStaffUsers(carwashId);

			if (!staff.Value.Any(x => x.Id != admin.Id))
			{
				return Result.Failure<List<Order>>("Вы не администратор данной автомойки");
			}
			var orders = await _ordersRepository.GetByCarwashIdWithPagination(carwashId, page, pageSize);
			return Result.Success(orders);
		}

		public async Task<Result<List<Order>>> GetOrdersOnCarwash(User admin, int carwashId)
		{
			var staff = await _carwashesRepository.GetStaffUsers(carwashId);

			if (!staff.Value.Any(x => x.Id == admin.Id))
			{
				return Result.Failure<List<Order>>("Вы не администратор данной автомойки");
			}
			var orders = await _ordersRepository.GetByCarwashId(carwashId);
			return Result.Success(orders);
		}

		public async Task<List<Order>> GetOrdersByUserId(int userId)
		{
			return await _ordersRepository.GetByUserId(userId);
		}

		public async Task<Result> ChangeStatus(User admin, int orderId, Status status)
		{
			var orderResult = await _ordersRepository.GetById(orderId);
			if (orderResult.IsFailure)
			{
				return Result.Failure(orderResult.Error);
			}
			var order = orderResult.Value;
			var staff = await _carwashesRepository.GetStaffUsers(order.CarwashId);
			if (!staff.Value.Any(x => x.Id == admin.Id))
			{
				return Result.Failure("Вы не сотрудник данной автомойки");
			}
			return await _ordersRepository.ChangeStatus(orderId, status);
		}

		public async Task<Result> ClientCancelOrder(int userId, int orderId, string reason)
		{
			var orderResult = await _ordersRepository.GetById(orderId);
			if (orderResult.IsFailure)
			{
				return Result.Failure(orderResult.Error);
			}
			var order = orderResult.Value;
			if (order.UserId != userId)
			{
				return Result.Failure("Вы не автор заказа");
			}
			var cancelResult = await _ordersRepository.ChangeStatusToCanceled(orderId, reason);
			return cancelResult;
		}
	}
}
