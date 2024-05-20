using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface IOrdersRepository
	{
		Task<Result> Add(int userId, int postId, string plateNumber, string carBrand, string carModel, int carReleaseYear, DateTime orderTime, List<int> servicesIds);
		Task<List<Order>> GetAll();
		Task<Result<Order>> GetById(int id);
		Task<List<Order>> GetByCarwashId(int carwashId);
		Task<List<Order>> GetByCarwashIdWithPagination(int carwashId, int page, int pageSize);
		Task<List<Order>> GetByUserId(int userId);
		Task<Result> ChangeStatus(int orderId, Status status);
		Task<Result> ChangeStatusToCanceled(int orderId, string reason);
		Task<Result<List<AvailableTimeForOrder>>> GetAvailableTimesForOrder(List<Service> services, DateOnly date, int carwashId);
	}
}