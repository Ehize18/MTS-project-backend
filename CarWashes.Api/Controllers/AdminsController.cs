using CarWashes.Api.Contracts;
using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarWashes.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AdminsController : ControllerBase
	{
		private readonly IUsersService _usersService;
		private readonly IHumansService _humansService;

		public AdminsController(IUsersService usersService, IHumansService humansService)
		{
			_usersService = usersService;
			_humansService = humansService;
		}

		[HttpPost]
		public async Task<ActionResult> AddUser([FromBody] AdminsRequest request)
		{
			Human human;
			User user;
			if (request.haveClientAccoint)
			{
				human = await _humansService.GetHumanByPhone(request.phone);
				user = new User(
				null, human.Id,
				"admin",
				Hash.SHA256Hash(request.login), Hash.SHA256Hash(request.password),
				null);
				await _usersService.AddUser(user);
				return Ok();
			}
			human = new Human(
				null,
				request.l_name, request.f_name, request.m_name,
				request.phone, request.birthday, request.email);
			user = new User(
				null, null,
				"admin",
				Hash.SHA256Hash(request.login), Hash.SHA256Hash(request.password),
				null);
			await _humansService.AddHumanWithUser(human, user);
			return Ok();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<HumanResponse>> GetHumanByID(int id)
		{
			var human = await _humansService.GetHumanByJwtToken(id);
			var respone = new HumanResponse(
				human.LastName, human.FirstName, human.MiddleName,
				human.Birthday, human.Phone, human.Email);
			return Ok(respone);
		}
	}
}
