using CarWashes.Core.Models;
using CarWashes.DataBase.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using CarWashes.Core.Interfaces;
using CSharpFunctionalExtensions;

namespace CarWashes.DataBase.Postgres.Repositories
{
	public class CarwashesRepository : ICarwashesRepository
	{
		private readonly CarWashesDbContext _dbContext;
		public CarwashesRepository(CarWashesDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Carwash>> GetAll()
		{
			var carwashesEntities = await _dbContext.Carwashes
				.AsNoTracking()
				.OrderBy(x => x.Id)
				.ToListAsync();
			var carwashes = carwashesEntities
				.Select(x => new Carwash(
					x.Id,
					x.OrgName, x.Name,
					x.City, x.Address,
					x.Phone, x.Email,
					x.WorkTimeStart, x.WorkTimeEnd))
				.ToList();
			return carwashes;
		}

		public async Task<Result<Carwash>> GetById(int id)
		{
			var carwashEntity = await _dbContext.Carwashes
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
			if (carwashEntity == null)
			{
				return Result.Failure<Carwash>("Автомойка не найдена");
			}
			var carwash = new Carwash(
				carwashEntity.Id,
				carwashEntity.OrgName, carwashEntity.Name,
				carwashEntity.City, carwashEntity.Address,
				carwashEntity.Phone, carwashEntity.Email,
				carwashEntity.WorkTimeStart, carwashEntity.WorkTimeEnd);
			return Result.Success<Carwash>(carwash);
		}

		public async Task Add(Carwash carwash, User user)
		{
			var carwashEntity = new CarwashEntity
			{
				Id = carwash.Id,
				OrgName = carwash.OrgName,
				Name = carwash.Name,
				City = carwash.City,
				Address = carwash.Address,
				Phone = carwash.Phone,
				Email = carwash.Email,
				WorkTimeStart = carwash.WorkTimeStart,
				WorkTimeEnd = carwash.WorkTimeEnd
			};
			var userEntity = await _dbContext.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == user.Id);
			userEntity.Carwashes.Add(carwashEntity);
			_dbContext.Users.Attach(userEntity);
			_dbContext.Users.Update(userEntity);
			carwashEntity.Staff.Add(userEntity);
			await _dbContext.Carwashes.AddAsync(carwashEntity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Update(int id,
			string phone, string email,
			TimeOnly workTimeStart, TimeOnly workTimeEnd)
		{
			await _dbContext.Carwashes
				.Where(x => x.Id == id)
				.ExecuteUpdateAsync(s => s
					.SetProperty(x => x.Phone, x => phone)
					.SetProperty(x => x.Email, x => email)
					.SetProperty(x => x.WorkTimeStart, x => workTimeStart)
					.SetProperty(x => x.WorkTimeEnd, x => workTimeEnd));
		}

		public async Task AddStaff(Carwash carwash, User user)
		{
			var carwashEntity = await _dbContext.Carwashes
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == carwash.Id);
			var userEntity = await _dbContext.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == user.Id);

			carwashEntity.Staff.Add(userEntity);
			_dbContext.Carwashes.Update(carwashEntity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<Result<List<Human>>> GetStaffHumans(int carwashId)
		{
			var carwashEntity = await _dbContext.Carwashes
				.AsNoTracking()
				.Where(x => x.Id == carwashId)
				.Include(x => x.Staff)
				.ThenInclude(x => x.Human)
				.FirstOrDefaultAsync();
			if (carwashEntity == null)
			{
				return Result.Failure<List<Human>>("Автомойка не найдена");
			}
			var humans = carwashEntity.Staff.Select(x => new Human(
				x.Human.Id,
				x.Human.L_Name, x.Human.F_Name, x.Human.M_Name,
				x.Human.Phone, x.Human.Birthday, x.Human.Email
				)).ToList();
			return Result.Success<List<Human>>(humans);
		}

		public async Task<Result<List<User>>> GetStaffUsers(int carwashId)
		{
			var carwashEntity = await _dbContext.Carwashes
				.AsNoTracking()
				.Where(x => x.Id == carwashId)
				.Include(x => x.Staff)
				.FirstOrDefaultAsync();
			if (carwashEntity == null)
			{
				return Result.Failure<List<User>>("Автомойка не найдена");
			}
			var staffUsers = carwashEntity.Staff.Select(x => new User(
				x.Id, x.HumanId, x.Role,
				x.Login, x.Password,
				x.Vk_token
				)).ToList();
			return Result.Success<List<User>>(staffUsers);
		}

		public async Task<Result<User>> GetOwner(int carwashId)
		{
			var carwashEntity = await _dbContext.Carwashes
				.AsNoTracking()
				.Include(x => x.Staff)
				.FirstOrDefaultAsync(x => x.Id == carwashId);
			if (carwashEntity == null)
			{
				return Result.Failure<User>("Автомойка не найдена");
			}
			var ownerEntity = carwashEntity.Staff.First();
			var owner = new User(
				ownerEntity.Id, ownerEntity.HumanId,
				ownerEntity.Role,
				ownerEntity.Login, ownerEntity.Password,
				ownerEntity.Vk_token
				);
			return Result.Success<User>(owner);
		}

		public async Task<Result> AddPost(int carwashId, int internalNumber)
		{
			var carwashEntity = await _dbContext.Carwashes
				.AsNoTracking()
				.Include(x => x.Posts)
				.FirstOrDefaultAsync(x =>x.Id == carwashId);
			if (carwashEntity == null)
			{
				return Result.Failure("Автомойка не найдена");
			}
			if (carwashEntity.Posts.Any(x => x.InternalNumber == internalNumber))
			{
				return Result.Failure("Пост с таким номером уже существует");
			}
			var postEntity = new PostEntity()
			{
				Id = null,
				InternalNumber = internalNumber,
				CarWashId = carwashId
			};
			carwashEntity.Posts.Add(postEntity);
			_dbContext.Update(carwashEntity);
			await _dbContext.SaveChangesAsync();
			return Result.Success();
		}

		public async Task<Result<List<Post>>> GetPosts(int carwashId)
		{
			var carwashEntity = await _dbContext.Carwashes
				.AsNoTracking()
				.Where(x => x.Id == carwashId)
				.Include(x => x.Posts)
				.FirstOrDefaultAsync();
			if (carwashEntity == null)
			{
				return Result.Failure<List<Post>>("Автомойка не найдена");
			}
			return Result.Success(carwashEntity.Posts.Select(x => new Post(
				(int)x.Id, x.CarWashId, x.InternalNumber)).ToList());
		}

		public async Task<Result> AddService(Service service)
		{
			var carwashEntity = await _dbContext.Carwashes
				.AsNoTracking()
				.Include(x => x.Services)
				.FirstOrDefaultAsync(x => x.Id == service.CarwashId);
			if (carwashEntity == null)
			{
				return Result.Failure("Автомойка не найдена");
			}
			var serviceEntity = new ServiceEntity()
			{
				CarwashId = service.CarwashId,
				Name = service.Name,
				Price = service.Price,
				Duration = service.Duration,
			};
			if (carwashEntity.Services.Any(x => x.Name == serviceEntity.Name))
			{
				return Result.Failure("Услуга с таким именем уже существует");
			}
			carwashEntity.Services.Add(serviceEntity);
			_dbContext.Update(carwashEntity);
			await _dbContext.SaveChangesAsync();
			return Result.Success();
		}

		public async Task<Result<List<Service>>> GetServices(int carwashId)
		{
			var carwashEntity = await _dbContext.Carwashes
				.AsNoTracking()
				.Include(x => x.Services)
				.FirstOrDefaultAsync(x => x.Id == carwashId);
			if (carwashEntity == null)
			{
				return Result.Failure<List<Service>>("Автомойка не найдена");
			}
			var services = carwashEntity.Services.Select(x => new Service(
				x.Id, x.CarwashId,
				x.Name, x.Price,
				x.Duration
				)).ToList();
			return Result.Success(services);
		}
	}
}
