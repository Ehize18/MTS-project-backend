using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

namespace CarWashes.Core.Interfaces
{
	public interface IServicesRepository
	{
		Task Add(Service service);
		Task<List<Service>> GetAll();
		Task<List<Service>> GetByCarwashId(int carwashId);
		Task<Result<Service>> GetById(int id);
		Task Update(int id, string name, decimal price, TimeSpan duration);
	}
}