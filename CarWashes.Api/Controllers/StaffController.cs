using CarWashes.Api.Contracts.Carwashes;
using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using CarWashes.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWashes.Api.Controllers
{
	[ApiController]
	[Route("Carwashes/{id:int}")]
	public class StaffController : ControllerBase
	{
		private readonly ICarwashesService _carwashesService;
		private readonly IUsersService _usersService;
		private readonly IHumansService _humansService;
		private readonly IJwtProvider _jwtProvider;

		public StaffController(
			ICarwashesService carwashesService, IJwtProvider jwtProvider,
			IUsersService usersService, IHumansService humansService)
		{
			_carwashesService = carwashesService;
			_jwtProvider = jwtProvider;
			_usersService = usersService;
			_humansService = humansService;
		}

		[Authorize]
		[HttpPost("staff")]
		public async Task<ActionResult> AddStaff([FromBody] CarwashesAddStaffRequest request, int id)
		{
			var token = HttpContext.Request.Cookies["milk-cookies"];
			var adminResult = await Helper.GetAdminByToken(token, _jwtProvider, _usersService);
			if (adminResult.IsFailure)
			{
				return BadRequest(adminResult.Error);
			}
			var admin = adminResult.Value;
			var ownerResult = await _carwashesService.GetOwner(id);
			var owner = ownerResult.Value;
			if (admin.Id != owner.Id)
			{
				return BadRequest("Вы не владелец автомойки");
			}
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
			var carwashResult = await _carwashesService.GetCarwashById(id);
			if (carwashResult.IsFailure)
			{
				return NotFound(id);
			}
			await _carwashesService.AddStaff(carwashResult.Value, newStaffResult.Value);
			return Ok();
		}

		[Authorize]
		[HttpGet("staff")]
		public async Task<ActionResult<List<Human>>> GetStaff(int id)
		{
			var token = HttpContext.Request.Cookies["milk-cookies"];
			var adminResult = await Helper.GetAdminByToken(token, _jwtProvider, _usersService);
			if (adminResult.IsFailure)
			{
				return BadRequest(adminResult.Error);
			};
			var admin = adminResult.Value;
			var staffResult = await _carwashesService.GetStaffByCarwashId(id);
			if (staffResult.IsFailure)
			{
				return NotFound(staffResult.Error);
			}
			var staff = staffResult.Value;
			if (!staff.Any(x => x.Id == admin.HumanId))
			{
				return BadRequest("Вы не сотрудник автомойки");
			}
			return Ok(staff);
		}
	}
}
