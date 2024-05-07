using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using CSharpFunctionalExtensions;

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

		public async Task<Result<Carwash>> GetCarwashById(int id)
		{
			return await _carwashesRepository.GetById(id);
		}

		public async Task AddStaff(Carwash carwash, User user)
		{
			await _carwashesRepository.AddStaff(carwash, user);
		}

		public async Task<Result<List<Human>>> GetStaffByCarwashId(int id)
		{
			return await _carwashesRepository.GetStaffHumans(id);
		}

		public async Task<Result<User>> GetOwner(int carwashId)
		{
			return await _carwashesRepository.GetOwner(carwashId);
		}

		public async Task<Result> AddPost(int carwashId, int internalNumber)
		{
			return await _carwashesRepository.AddPost(carwashId, internalNumber);
		}

		public async Task<Result<List<Post>>> GetPosts(int carwashId, int staffId)
		{
			var staffUsersResult = await _carwashesRepository.GetStaffUsers(carwashId);
			if (staffUsersResult.IsFailure)
			{
				return Result.Failure<List<Post>>(staffUsersResult.Error);
			}
			var staffUsers = staffUsersResult.Value;
			if (!staffUsers.Any(x => x.Id == staffId))
			{
				return Result.Failure<List<Post>>("Вы не сотрудник данной автомойки");
			}
			var postsResult = await _carwashesRepository.GetPosts(carwashId);
			return postsResult;
		}

		public async Task<Result> AddService(User staff, int carwashId, string name, decimal price, TimeSpan duration)
		{
			var staffResult = await _carwashesRepository.GetStaffUsers(carwashId);
			if (staffResult.IsFailure)
			{
				return Result.Failure(staffResult.Error);
			}
			if (!staffResult.Value.Any(x => x.Id == staff.Id))
			{
				return Result.Failure("Вы не сотрудник автомойки");
			}
			var service = new Service(
				null, carwashId,
				name, price,
				duration);
			return await _carwashesRepository.AddService(service);
		}

		public async Task<Result<List<Service>>> GetServices(int carwashId)
		{
			return await _carwashesRepository.GetServices(carwashId);
		}
	}
}
