using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

using Echo.Models;
using Echo.Services;

namespace Echo.Controllers
{
    [Route("/api/chats")]
	[ApiController]
    public class ChatAPIController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatAPIController(ChatService chatService)
        {
			_chatService = chatService;
        }
		
        [HttpPost]
        [Route("create", Name = "CreateChat")]
		public async Task<IActionResult> CreateChat([FromBody] CreateChatDto dto)
		{
			try
			{
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
