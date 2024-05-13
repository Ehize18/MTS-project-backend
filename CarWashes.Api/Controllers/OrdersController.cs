using CarWashes.Api.Contracts.Carwashes.Orders;
using CarWashes.Core.Interfaces;
using CarWashes.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWashes.Api.Controllers
{
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly IOrdersService _ordersService;
		private readonly ICarwashesService _carwashesService;
		private readonly IJwtProvider _jwtProvider;
		private readonly IUsersService _usersService;

		public OrdersController(IOrdersService ordersService, ICarwashesService carwashesService, IJwtProvider jwtProvider, IUsersService usersService)
		{
			_ordersService = ordersService;
			_carwashesService = carwashesService;
			_jwtProvider = jwtProvider;
			_usersService = usersService;
		}

		[Route("Carwashes/{id:int}/orders/availabletimes")]
		[HttpPost]
		public async Task<ActionResult> GetAvailableTimesForOrder([FromBody] AvailableTimesRequest request, int id)
		{
			var timesResult = await _ordersService.GetAvailableTimesForOrder(request.ServiceIds, request.date, id);
			if (timesResult.IsFailure)
			{
				return BadRequest(timesResult.Error);
			}
			return Ok(timesResult.Value);
		}

		[Authorize]
		[Route("Carwashes/{id:int}/orders")]
		[HttpPost]
		public async Task<ActionResult> CreateOrder([FromBody] OrderCreateRequest request, int id)
		{
			var token = HttpContext.Request.Cookies["milk-cookies"];
			var adminResult = await Helper.GetAdminByToken(token, _jwtProvider, _usersService);
			if (adminResult.IsSuccess)
			{
				return BadRequest("Вы админ");
			}
			var userid = _jwtProvider.GetId(token);
			var orderResult = await _ordersService.AddOrder(userid, request.PostId, request.PlateNumber, request.CarBrand, request.CarModel, request.CarReleaseYear, request.OrderTime, request.ServiceIds);
			if (orderResult.IsFailure)
			{
				return BadRequest(orderResult.Error);
			}
			return Ok();
		}
	}
}
