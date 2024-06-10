using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Echo.Data;
using Echo.Services;

namespace Echo.Controllers
{
    [Route("/friends")]
    public class FriendshipController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<ProfileController> _localizer;
        private readonly UserService _userService;
		private readonly TimeZoneInfo _timeZone;

        public FriendshipController(AppDbContext context, IStringLocalizer<ProfileController> localizer, UserService userService, TimeZoneInfo timeZone)
        {
            _context = context;
            _localizer = localizer;
			_userService = userService;
			_timeZone = timeZone;
        }

        [HttpGet]
        [Route("", Name = "Friends")]
        public async Task<IActionResult> Index()
        {
			var userResult = await _userService.GetUserAsync(User);
			var user = userResult.Result;
			ViewBag.Id = user.Id;
			var friendsResult = await _userService.GetAllFriendsAsync(User);
			return View(friendsResult.Result);
		}

        [HttpGet]
        [Route("incoming", Name = "IncomingFriendships")]
        public async Task<IActionResult> Incoming()
        {
			var userResult = await _userService.GetUserAsync(User);
			var user = userResult.Result;
			ViewBag.Id = user.Id;
			var friendsResult = await _userService.GetAllIncomingFriendshipsAsync(User);
			return View(friendsResult.Result);
		}

        [HttpGet]
        [Route("outgoing", Name = "OutgoingFriendships")]
        public async Task<IActionResult> Outgoing()
        {
			var userResult = await _userService.GetUserAsync(User);
			var user = userResult.Result;
			ViewBag.Id = user.Id;
			var friendsResult = await _userService.GetAllOutgoingFriendshipsAsync(User);
			return View(friendsResult.Result);
		}
	}
}
