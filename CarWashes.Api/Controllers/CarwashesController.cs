using CarWashes.Api.Contracts;
using CarWashes.Api.Contracts.Carwashes;
using CarWashes.Application.Services;
using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CSharpFunctionalExtensions;
using CarWashes.Infrastructure;

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
		public async Task<ActionResult> AddCarwash([FromBody] CarwashesCreateRequest request)
		{
			var token = HttpContext.Request.Cookies["milk-cookies"];
			var adminResult = await Helper.GetAdminByToken(token, _jwtProvider, _usersService);
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

		[HttpGet]
		public async Task<ActionResult<List<CarwashesResponse>>> GetAllCarwashes()
		{
			var carwashes = await _carwashesService.GetAllCarwashes();
			var carwashesResponse = carwashes.Select(x => new CarwashesResponse(
				(int)x.Id,
				x.OrgName, x.Name,
				x.City, x.Address,
				x.Phone, x.Email,
				x.WorkTimeStart, x.WorkTimeEnd
				)).ToList();
			return Ok(carwashesResponse);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<CarwashesResponse>> GetCarwashById(int id)
		{
			var carwashResult = await _carwashesService.GetCarwashById(id);
			if (carwashResult.IsFailure)
			{
				return NotFound(id);
			}
			var carwash = carwashResult.Value;
			var carwashResponse = new CarwashesResponse(
				(int)carwash.Id,
				carwash.OrgName, carwash.Name,
				carwash.City, carwash.Address,
				carwash.Phone, carwash.Email,
				carwash.WorkTimeStart, carwash.WorkTimeEnd
				);
			return Ok(carwashResponse);
		}
	}
}
