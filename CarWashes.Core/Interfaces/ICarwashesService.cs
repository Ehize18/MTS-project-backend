using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface ICarwashesService
	{
		Task AddCarwash(Carwash carwash, User user);
		Task<List<Carwash>> GetAllCarwashes();
		Task<Result<Carwash>> GetCarwashById(int id);
		Task AddStaff(Carwash carwash, User user);
		Task<Result<List<Human>>> GetStaffByCarwashId(int id);
		Task<Result<User>> GetOwner(int carwashId);
		Task<Result> AddPost(int carwashId, int internalNumber);
		Task<Result<List<Post>>> GetPosts(int carwashId, int staffId);
		Task<Result> AddService(User staff, int carwashId, string name, decimal price, TimeSpan duration);
		Task<Result<List<Service>>> GetServices(int carwashId);
	}
}