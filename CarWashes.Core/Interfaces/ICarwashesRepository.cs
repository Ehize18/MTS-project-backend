using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface ICarwashesRepository
	{
		Task Add(Carwash carwash, User user);
		Task<List<Carwash>> GetAll();
		Task<Result<Carwash>> GetById(int id);
		Task Update(int id, string phone, string email, TimeOnly workTimeStart, TimeOnly workTimeEnd);
		Task AddStaff(Carwash carwash, User user);
		Task<Result<List<Human>>> GetStaffHumans(int carwashId);
		Task<Result<List<User>>> GetStaffUsers(int carwashId);
		Task<Result<User>> GetOwner(int carwashId);
		Task<Result> AddPost(int carwashId, int internalNumber);
		Task<Result<List<Post>>> GetPosts(int carwashId);
		Task<Result> AddService(Service service);
		Task<Result<List<Service>>> GetServices(int carwashId);
	}
}