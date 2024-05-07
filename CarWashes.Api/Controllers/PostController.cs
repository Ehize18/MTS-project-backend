using CarWashes.Api.Contracts.Carwashes;
using CarWashes.Core.Interfaces;
using CarWashes.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWashes.Api.Controllers
{
	[ApiController]
	[Route("Carwashes/{id:int}")]
	public class PostsController : ControllerBase
	{
		private readonly ICarwashesService _carwashesService;
		private readonly IUsersService _usersService;
		private readonly IJwtProvider _jwtProvider;

		public PostsController(
			ICarwashesService carwashesService, IJwtProvider jwtProvider,
			IUsersService usersService, IHumansService humansService)
		{
			_carwashesService = carwashesService;
			_jwtProvider = jwtProvider;
			_usersService = usersService;
		}

		[Authorize]
		[HttpPost("posts")]
		public async Task<ActionResult> AddPost([FromBody] CarwashesAddPostRequest request, int id)
		{
			var token = HttpContext.Request.Cookies["milk-cookies"];
			var adminResult = await Helper.GetAdminByToken(token, _jwtProvider, _usersService);
			if (adminResult.IsFailure)
			{
				return BadRequest(adminResult.Error);
			}
			var ownerResult = await _carwashesService.GetOwner(id);
			if (ownerResult.IsFailure)
			{
				return NotFound(ownerResult.Error);
			}
			if (adminResult.Value.Id != ownerResult.Value.Id)
			{
				return BadRequest("Вы не владелец автомойки");
			}
			var postResult = await _carwashesService.AddPost(id, request.internalNumber);
			if (postResult.IsFailure)
			{
				return BadRequest(postResult.Error);
			}
			return Ok();
		}

		[Authorize]
		[HttpGet("posts")]
		public async Task<ActionResult> GetPosts(int id)
		{
			var token = HttpContext.Request.Cookies["milk-cookies"];
			var adminResult = await Helper.GetAdminByToken(token, _jwtProvider, _usersService);
			if (adminResult.IsFailure)
			{
				return BadRequest(adminResult.Error);
			}
			var postsResult = await _carwashesService.GetPosts(id, (int)adminResult.Value.Id);
			if (postsResult.IsFailure)
			{
				return BadRequest(postsResult.Error);
			}
			var dct = new Dictionary<int, object>()
			{
				{postsResult.Value.Count, postsResult.Value}
			};
			return Ok(dct);
		}
	}
}
