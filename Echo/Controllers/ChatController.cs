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

        public ChatController(AppDbContext context, IStringLocalizer<ProfileController> localizer, UserService userService, ChatService chatService)
        {
            _context = context;
            _localizer = localizer;
			_userService = userService;
			_chatService = chatService;
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
			var chats = await _chatService.GetOrderedChatsAsync(User);
			return View(chats.Result);
		}
	}
}
