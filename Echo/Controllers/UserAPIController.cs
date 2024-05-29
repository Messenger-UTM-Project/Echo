using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

using Echo.Roles;
using Echo.Models;
using Echo.Services;

namespace Echo.Controllers
{
	[Route("/api/user")]
	[ApiController]
	public class UserAPIController : ControllerBase
	{
		private readonly UserService _userService;
		private readonly AppRoleManager _roleManager;

		public UserAPIController(UserService userService, AppRoleManager roleManager)
		{
			_userService = userService;
			_roleManager = roleManager;
		}

		[HttpPost]
		[Route("getCurrentUser", Name = "GetCurrentUser")]
		public async Task<IActionResult> getCurrentUser()
		{
			var userResult = await _userService.GetUserAsync(User);
			return Ok( new { 
					Id = userResult.Result.Id, 
					username = userResult.Result.UserName, 
					CreatedAt = userResult.Result.CreatedAt, 
					ProfileImagePath = userResult.Result.ProfileImagePath 
				});
		}

		[HttpPost]
		[Route("getCurrentUuid", Name = "GetCurrentUuid")]
		public async Task<IActionResult> getCurrentUuid()
		{
			var userResult = await _userService.GetUserAsync(User);
			return Ok(new { Id = userResult.Result.Id });
		}
	}
}
