using CarWashes.Api.Contracts;
using CarWashes.Api.Contracts.Carwashes;
using CarWashes.Application.Services;
using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CSharpFunctionalExtensions;

namespace CarWashes.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CarwashesController : ControllerBase
	{
		private readonly ICarwashesService _carwashesService;
		private readonly IUsersService _usersService;
		private readonly IHumansService _humansService;
		private readonly IJwtProvider _jwtProvider;

		public CarwashesController(
			ICarwashesService carwashesService, IJwtProvider jwtProvider,
			IUsersService usersService, IHumansService humansService)
		{
			_carwashesService = carwashesService;
			_jwtProvider = jwtProvider;
			_usersService = usersService;
			_humansService = humansService;
		}

		[Authorize]
		[HttpPost]
		public async Task<ActionResult> AddCarwash([FromBody] CarwashesRequest request)
		{
			var token = HttpContext.Request.Cookies["choco-cookies"];
			var adminResult = await GetAdminByToken(token);
			if (adminResult.IsFailure)
			{
				return BadRequest(adminResult.Error);
			}
			var admin = adminResult.Value;
			var carwash = new Carwash(
				null,
				request.orgName, request.name,
				request.city, request.address,
				request.phone, request.email,
				request.workTimeStart, request.workTimeEnd
				);
			await _carwashesService.AddCarwash(carwash, admin);
			return Ok();
		}

		[Route("addstaff")]
		[Authorize]
		[HttpPost]
		public async Task<ActionResult> AddStaff([FromBody] CarwashesAddStaffRequest request)
		{
			var token = HttpContext.Request.Cookies["choco-cookies"];
			var adminResult = await GetAdminByToken(token);
			if (adminResult.IsFailure)
			{
				return BadRequest(adminResult.Error);
			}
			var admin = adminResult.Value;
			var humanResult = await _humansService.GetHumanByPhone(request.staffPhone);
			if (humanResult.IsFailure)
			{
				return BadRequest("Сотрудник не найден");
			}
			var newStaffResult = await _usersService.GetAdminByHumanId((int)humanResult.Value.Id);
			if (newStaffResult.IsFailure)
			{
				return BadRequest(newStaffResult.Error);
			}
			var carwash = await _carwashesService.GetCarwashById(request.carwashId);
			await _carwashesService.AddStaff(carwash, newStaffResult.Value);
			return Ok();
		}

		private async Task<Result<User>> GetAdminByToken(string token)
		{
			if (token == null)
			{
				return Result.Failure<User>("Ошибка авторизации");
			}
			var id = _jwtProvider.GetId(token);
			if (_jwtProvider.GetRole(token) != "admin")
			{
				Console.WriteLine(_jwtProvider.GetRole(token));
				return Result.Failure<User>("Ошибка авторизации");
			}
			var userResult = await _usersService.GetUserById(id);
			if (userResult.IsFailure)
			{
				return Result.Failure<User>("Ошибка авторизации");
			}
			return Result.Success<User>(userResult.Value);
		}
	}
}
