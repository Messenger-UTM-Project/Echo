using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Echo.Data;
using Echo.Services;

namespace Echo.Controllers
{
    [Route("/chats")]
    public class ChatController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<ProfileController> _localizer;
        private readonly UserService _userService;
        private readonly ChatService _chatService;
		private readonly TimeZoneInfo _timeZone;

        public ChatController(AppDbContext context, IStringLocalizer<ProfileController> localizer, UserService userService, ChatService chatService, TimeZoneInfo timeZone)
        {
            _context = context;
            _localizer = localizer;
			_userService = userService;
			_chatService = chatService;
			_timeZone = timeZone;
        }

        [HttpGet]
        [Route("", Name = "Chats")]
        public async Task<IActionResult> Index()
        {
			var chats = await _chatService.GetOrderedChatsAsync(User);
			return View(chats.Result);
		}

        [HttpGet]
        [Route("{guid:guid}", Name = "Chat")]
        public async Task<IActionResult> Chat(Guid guid)
        {
			var userResult = await _userService.GetUserAsync(User);
			ViewBag.User = userResult.Result;
			ViewBag.TimeZone = _timeZone;
			var chats = await _chatService.GetChatByIdAsync(guid, true);
			return View(chats.Result);
		}
	}
}
