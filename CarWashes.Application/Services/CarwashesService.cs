using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;

namespace CarWashes.Application.Services
{
	public class CarwashesService : ICarwashesService
	{
		private readonly ICarwashesRepository _carwashesRepository;
		private readonly IUsersRepository _usersRepository;

		public CarwashesService(ICarwashesRepository repository, IUsersRepository usersRepository)
		{
			_carwashesRepository = repository;
			_usersRepository = usersRepository;
		}

		public async Task AddCarwash(Carwash carwash, User user)
		{
			await _carwashesRepository.Add(carwash, user);
		}

		public async Task<List<Carwash>> GetAllCarwashes()
		{
			return await _carwashesRepository.GetAll();
		}

		public async Task<Carwash> GetCarwashById(int id)
		{
			return await _carwashesRepository.GetById(id);
		}

		public async Task AddStaff(Carwash carwash, User user)
		{
			await _carwashesRepository.AddStaff(carwash, user);
		}
	}
}
