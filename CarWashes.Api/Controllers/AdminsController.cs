using CarWashes.Api.Contracts;
using CarWashes.Api.Contracts.Admins;
using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CarWashes.Api.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class AdminsController : ControllerBase
	{
		private readonly IUsersService _usersService;
		private readonly IHumansService _humansService;
		private readonly IJwtProvider _jwtProvider;

		public AdminsController(IUsersService usersService, IHumansService humansService, IJwtProvider jwtProvider)
		{
			_usersService = usersService;
			_humansService = humansService;
			_jwtProvider = jwtProvider;
		}
		[Route("register")]
		[HttpPost]
		public async Task<ActionResult> AddUser([FromBody] AdminsRegisterRequest request)
		{
			Human human;
			User user;
			Result registerResult;
			if (request.haveClientAccount)
			{
				var result = await _humansService.GetHumanByPhone(request.phone);
				if (result.IsFailure) 
				{
					return NotFound("Пользователь с таким номером не найден");
				}
				human = result.Value;
				user = new User(
				null, human.Id,
				"admin",
				Hash.SHA256Hash(request.login), Hash.SHA256Hash(request.password),
				null);
				registerResult = await _usersService.AddUser(user);
				if (registerResult.IsFailure)
				{
					return BadRequest(registerResult.Error);
				}
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
			registerResult = await _humansService.AddHumanWithUser(human, user);
			if (registerResult.IsFailure)
			{
				return BadRequest(registerResult.Error);
			}
			return Ok();
		}

		[Route("login")]
		[HttpPost]
		public async Task<ActionResult> Login(AdminsLoginRequest request)
		{
			var result = await _usersService.Login(Hash.SHA256Hash(request.login), Hash.SHA256Hash(request.password));
			if (result.IsFailure)
			{
				return BadRequest(result.Error);
			}
			var token = result.Value;

			HttpContext.Response.Cookies.Append("choco-cookies", token);

			return Ok(token);
		}

		[Authorize]
		[HttpGet]
		public async Task<ActionResult<HumanResponse>> GetHumanByID()
		{
			var token = HttpContext.Request.Cookies["choco-cookies"];
			var id = _jwtProvider.GetId(token);
			var human = await _humansService.GetHumanById(id);
			var respone = new HumanResponse(
				human.LastName, human.FirstName, human.MiddleName,
				human.Birthday, human.Phone, human.Email);
			return Ok(respone);
		}
	}
}
