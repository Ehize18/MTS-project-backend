using CarWashes.Api.Contracts.Carwashes.Orders;
using CarWashes.Core;
using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using CarWashes.Infrastructure;
using CSharpFunctionalExtensions;
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

		[Authorize]
		[Route("Carwashes/{id:int}/orders")]
		[HttpGet]
		public async Task<ActionResult> GetOrdersByCarwashId(int id, int page, int pageSize)
		{
			var token = HttpContext.Request.Cookies["milk-cookies"];
			var adminResult = await Helper.GetAdminByToken(token, _jwtProvider, _usersService);
			if (adminResult.IsFailure)
			{
				Console.WriteLine("1");
				return Forbid();
			}
			var ordersResult = new Result<List<Order>>();
			if (page == 0 || pageSize == 0)
			{
				ordersResult = await _ordersService.GetOrdersOnCarwash(adminResult.Value, id);
			}
			else
			{
				ordersResult = await _ordersService.GetOrdersOnCarwashWithPagination(adminResult.Value, id, page, pageSize);
			}
            if (ordersResult.IsFailure)
            {
				Console.WriteLine("2");
				return Forbid();
            }
			var orders = ordersResult.Value;
			var ordersResponse = orders.Select(o => new OrderForAdminResponse(
				o.Id, o.FirstName, o.Phone, o.PostInternalNumber,
				o.PlateNumber, o.CarBrand, o.CarModel, o.CarReleaseYear,
				o.Status, o.OrderTime, o.CreatedAt, o.UpdatedAt
				));
			foreach ( var order in ordersResponse)
			{
				Console.WriteLine(order.Status);
			}
			return Ok(ordersResponse);
		}

		[Authorize]
		[Route("Carwashes/{id:int}/orders")]
		[HttpPut]
		public async Task<ActionResult> AdminChangeOrderStatus([FromBody] OrderChangeRequest request)
		{
			var token = HttpContext.Request.Cookies["milk-cookies"];
			var adminResult = await Helper.GetAdminByToken(token, _jwtProvider, _usersService);
			if (adminResult.IsFailure)
			{
				return Forbid();
			}
			var admin = adminResult.Value;
			var changeResult = await _ordersService.ChangeStatus(admin, request.OrderId, request.Status);
            if (changeResult.IsFailure)
            {
				return BadRequest(changeResult.Error);
            }
			return Ok();
        }

		[Authorize]
		[Route("Clients/orders")]
		[HttpGet]
		public async Task<ActionResult> GetOrdersByUserId()
		{
			var token = HttpContext.Request.Cookies["milk-cookies"];
			var userId = _jwtProvider.GetId(token);
			var orders = await _ordersService.GetOrdersByUserId(userId);
			var ordersResponse = orders.Select(o => new OrderForClientResponse(
				o.Id,
				o.PlateNumber, o.CarBrand, o.CarModel, o.CarReleaseYear,
				o.Status, o.OrderTime, o.UpdatedAt
				));
			return Ok(ordersResponse);
		}

		[Authorize]
		[Route("Clients/orders")]
		[HttpPut]
		public async Task<ActionResult> ClientCancelOrder([FromBody] OrderCancelRequest request)
		{
			var token = HttpContext.Request.Cookies["milk-cookies"];
			var userId = _jwtProvider.GetId(token);
			var cancelResult = await _ordersService.ClientCancelOrder(userId, request.Id, request.Reason);
			if (cancelResult.IsFailure)
			{
				return BadRequest(cancelResult.Error);
			}
			return Ok();
		}
	}
}
