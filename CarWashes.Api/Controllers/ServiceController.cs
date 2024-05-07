using CarWashes.Api.Contracts.Carwashes;
using CarWashes.Core.Interfaces;
using CarWashes.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWashes.Api.Controllers
{
	[ApiController]
	[Route("Carwashes/{id:int}")]
	public class ServicesController : ControllerBase
	{
		private readonly ICarwashesService _carwashesService;
		private readonly IUsersService _usersService;
		private readonly IJwtProvider _jwtProvider;

		public ServicesController(
			ICarwashesService carwashesService, IJwtProvider jwtProvider,
			IUsersService usersService)
		{
			_carwashesService = carwashesService;
			_jwtProvider = jwtProvider;
			_usersService = usersService;
		}

		[Authorize]
		[HttpPost("services")]
		public async Task<ActionResult> AddService([FromBody] CarwashesAddServiceRequest request, int id)
		{
			var token = HttpContext.Request.Cookies["milk-cookies"];
			var adminResult = await Helper.GetAdminByToken(token, _jwtProvider, _usersService);
			if (adminResult.IsFailure)
			{
				return BadRequest(adminResult.Error);
			}
			var serviceResult = await _carwashesService.AddService(adminResult.Value, id, request.name, request.price, request.duration);
			if (serviceResult.IsFailure)
			{
				return BadRequest(serviceResult.Error);
			}
			return Ok();
		}

		[HttpGet("services")]
		public async Task<ActionResult> GetServices(int id)
		{
			var serviceResult = await _carwashesService.GetServices(id);
			if (serviceResult.IsFailure)
			{
				return NotFound(serviceResult.Error);
			}
			var serviceResponses = serviceResult.Value.Select(x => new CarwashesServiceResponse(
				(int)x.Id, x.Name, x.Price, x.Duration)).ToList();
			return Ok(serviceResponses);
		}
	}
}
