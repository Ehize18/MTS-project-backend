﻿using CarWashes.Api.Contracts;
using CarWashes.Api.Contracts.Clients;
using CarWashes.Core.Interfaces;
using CarWashes.Core.Models;
using CarWashes.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarWashes.Api.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class ClientsController : ControllerBase
	{
		private readonly IUsersService _usersService;
		private readonly IHumansService _humansService;
		private readonly IJwtProvider _jwtProvider;

		public ClientsController(IUsersService usersService, IHumansService humansService, IJwtProvider jwtProvider)
		{
			_usersService = usersService;
			_humansService = humansService;
			_jwtProvider = jwtProvider;
		}
		[Route("register")]
		[HttpPost]
		public async Task<ActionResult> AddUser([FromBody] ClientsRegisterRequest request)
		{
			var human = new Human(
				null,
				request.l_name, request.f_name, request.m_name,
				request.phone, request.birthday, request.email);
			var user = new User(
				null, null,
				"client",
				Helper.SHA256Hash(request.login), Helper.SHA256Hash(request.password),
				null);
			var registerResult = await _humansService.AddHumanWithUser(human, user);
			if (registerResult.IsFailure)
			{
				return BadRequest(registerResult.Error);
			}
			return Ok();
		}

		[Authorize]
		[HttpGet]
		public async Task<ActionResult<HumanResponse>> GetHumanByID()
		{
			var token = HttpContext.Request.Cookies["milk-cookies"];
			var id = _jwtProvider.GetId(token);
			var human = await _humansService.GetHumanById(id);
			var respone = new HumanResponse(
				human.LastName, human.FirstName, human.MiddleName,
				human.Birthday, human.Phone, human.Email);
			return Ok(respone);
		}

		[Route("login")]
		[HttpPost]
		public async Task<ActionResult> Login(ClientsLoginRequest request)
		{
			var result = await _usersService.Login(Helper.SHA256Hash(request.login), Helper.SHA256Hash(request.password));
			if (result.IsFailure)
			{
				return BadRequest(result.Error);
			}
			var token = result.Value;

			HttpContext.Response.Cookies.Append("milk-cookies", token);

			return Ok(token);
		}
	}
}
