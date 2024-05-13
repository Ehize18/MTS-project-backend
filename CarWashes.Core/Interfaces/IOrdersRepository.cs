using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface IOrdersRepository
	{
		Task<Result> Add(Order order, List<int> servicesIds);
		Task<List<Order>> GetAll();
		Task<List<Order>> GetByCarwashId(int carwashId);
		Task<List<Order>> GetByUserId(int userId);
		Task Update(int orderId, Status status, DateTime updatadAt);
		Task<Result<List<AvailableTimeForOrder>>> GetAvailableTimesForOrder(List<Service> services, DateOnly date, int carwashId);
	}
}