using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

using Echo.Roles;
using Echo.Services;

namespace Echo.Controllers
{
    [Route("/api/chats")]
	[ApiController]
    public class ChatAPIController : ControllerBase
    {
		private readonly UserService _userService;
        private readonly ChatService _chatService;
		private readonly AppRoleManager _roleManager;

        public ChatAPIController(UserService userService, ChatService chatService, AppRoleManager roleManager)
        {
			_userService = userService;
			_chatService = chatService;
			_roleManager = roleManager;
        }
		
        [HttpPost]
        [Route("create", Name = "CreateChat")]
		public async Task<IActionResult> CreateChat([FromBody] CreateChatDto dto)
		{
			try
			{
				var userResult = await _userService.GetUserAsync(User);
				var isAdmin = await _roleManager.UserIsInRole(userResult.Result.Id, "Admin");
				if (!dto.OwnerUserIds.Contains(userResult.Result.Id) && !isAdmin)
					return BadRequest("Not allowed.");
				var chatResult = await _chatService.CreateChatAsync(dto.ChatName, dto.OwnerUserIds, dto.MemberUserIds);
				return Ok(chatResult);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}

public class CreateChatDto
{
	public string ChatName { get; set; }
	public List<Guid> OwnerUserIds { get; set; }
	public List<Guid> MemberUserIds { get; set; }
}
