using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface IOrdersService
	{
		Task<Result> AddOrder(int userId, int postId, string plateNumber, string carBrand, string carModel, int CarReleaseYear, DateTime orderTime, List<int> servicesIds);
		Task<Result<List<AvailableTimeForOrder>>> GetAvailableTimesForOrder(List<int> serviceIds, DateOnly date, int carwashId);
		Task<Result<List<Order>>> GetOrdersOnCarwashWithPagination(User admin, int carwashId, int page, int pageSize);
		Task<Result<List<Order>>> GetOrdersOnCarwash(User admin, int carwashId);
		Task<List<Order>> GetOrdersByUserId(int userId);
		Task<Result> ChangeStatus(User admin, int orderId, Status status);
		Task<Result> ClientCancelOrder(int userId, int orderId, string reason);
	}
}