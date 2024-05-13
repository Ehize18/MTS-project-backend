using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Application.Services
{
	public class OrdersService : IOrdersService
	{
		private readonly IOrdersRepository _ordersRepository;
		private readonly IServicesRepository _servicesRepository;

		public OrdersService(IOrdersRepository ordersRepository, IServicesRepository servicesRepository)
		{
			_ordersRepository = ordersRepository;
			_servicesRepository = servicesRepository;
		}

		public async Task<Result> AddOrder(int userId, int postId, string plateNumber, string carBrand, string carModel, int CarReleaseYear, DateTime orderTime, List<int> servicesIds)
		{
			var order = new Order(
				null, userId, postId,
				plateNumber, carBrand, carModel, CarReleaseYear,
				orderTime, DateTime.UtcNow, DateTime.UtcNow);
			var orderResult = await _ordersRepository.Add(order, servicesIds);
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
	}
}
