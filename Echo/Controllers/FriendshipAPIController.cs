using Microsoft.AspNetCore.Mvc;

using Echo.Roles;
using Echo.Services;

namespace Echo.Controllers
{
	[Route("/api/friendship")]
	[ApiController]
	public class FriendshipAPIController : ControllerBase
	{
		private readonly UserService _userService;
		private readonly FriendshipService _friendshipService;
		private readonly AppRoleManager _roleManager;

		public FriendshipAPIController(UserService userService, FriendshipService friendshipService, AppRoleManager roleManager)
		{
			_userService = userService;
			_friendshipService = friendshipService;
			_roleManager = roleManager;
		}

		[HttpPost]
		[Route("create", Name = "CreateFriendship")]
		public async Task<IActionResult> CreateFriendship([FromBody] CreateFriendshipDto dto)
		{
			try
			{
				if (dto.UserId == dto.FriendId)
					return BadRequest("Set different UUIDs.");
				var userResult = await _userService.GetUserAsync(User);
				var isAdmin = await _roleManager.UserIsInRole(userResult.Result.Id, "Admin");
				if (userResult.Result.Id != dto.UserId && !isAdmin)
					return BadRequest("Not allowed.");

				var friendshipResult = await _friendshipService.CreateFriendshipAsync(dto.UserId, dto.FriendId);
				return Ok(friendshipResult);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("accept", Name = "AcceptFriendship")]
		public async Task<IActionResult> AcceptFriendship([FromBody] ModifyFriendshipDto dto)
		{
			try
			{
				if (dto.UserId == dto.FriendId)
					return BadRequest("Set different UUIDs.");
				var userResult = await _userService.GetUserAsync(User);
				var isAdmin = await _roleManager.UserIsInRole(userResult.Result.Id, "Admin");
				if (userResult.Result.Id != dto.FriendId && !isAdmin)
					return BadRequest("Not allowed.");

				var friendshipResult = await _friendshipService.AcceptFriendshipAsync(dto.UserId, dto.FriendId);
				return Ok(friendshipResult);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("reject", Name = "RejectFriendship")]
		public async Task<IActionResult> RejectFriendship([FromBody] ModifyFriendshipDto dto)
		{
			try
			{
				if (dto.UserId == dto.FriendId)
					return BadRequest("Set different UUIDs.");
				var userResult = await _userService.GetUserAsync(User);
				var isAdmin = await _roleManager.UserIsInRole(userResult.Result.Id, "Admin");
				if (userResult.Result.Id != dto.FriendId && !isAdmin)
					return BadRequest("Not allowed.");

				var friendshipResult = await _friendshipService.RejectFriendshipAsync(dto.UserId, dto.FriendId);
				return Ok(friendshipResult);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("delete", Name = "DeleteFriendship")]
		public async Task<IActionResult> DeleteFriendship([FromBody] ModifyFriendshipDto dto)
		{
			try
			{
				if (dto.UserId == dto.FriendId)
					return BadRequest("Set different UUIDs.");
				var userResult = await _userService.GetUserAsync(User);
				var isAdmin = await _roleManager.UserIsInRole(userResult.Result.Id, "Admin");
				if (userResult.Result.Id != dto.FriendId && !isAdmin)
					return BadRequest("Not allowed.");

				await _friendshipService.DeleteFriendshipAsync(dto.UserId, dto.FriendId);
				return Ok(new {});
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}

public class CreateFriendshipDto
{
    public Guid UserId { get; set; }
    public Guid FriendId { get; set; }
}

public class ModifyFriendshipDto
{
    public Guid UserId { get; set; }
    public Guid FriendId { get; set; }
}
