using CarWashes.Core.Models;

namespace CarWashes.Core.Interfaces
{
	public interface ICarwashesRepository
	{
		Task Add(Carwash carwash, User user);
		Task<List<Carwash>> GetAll();
		Task<Carwash> GetById(int id);
		Task Update(int id, string phone, string email, TimeOnly workTimeStart, TimeOnly workTimeEnd);
		Task AddStaff(Carwash carwash, User user);
	}
}