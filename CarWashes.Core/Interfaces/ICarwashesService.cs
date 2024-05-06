using CarWashes.Core.Models;

namespace CarWashes.Core.Interfaces
{
	public interface ICarwashesService
	{
		Task AddCarwash(Carwash carwash, User user);
		Task<List<Carwash>> GetAllCarwashes();
		Task<Carwash> GetCarwashById(int id);
		Task AddStaff(Carwash carwash, User user);
	}
}