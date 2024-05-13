using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface IOrdersService
	{
		Task<Result> AddOrder(int userId, int postId, string plateNumber, string carBrand, string carModel, int CarReleaseYear, DateTime orderTime, List<int> servicesIds);
		Task<Result<List<AvailableTimeForOrder>>> GetAvailableTimesForOrder(List<int> serviceIds, DateOnly date, int carwashId);
	}
}