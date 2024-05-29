using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

using Echo.Data;
using Echo.Models;
using Echo.Services;

namespace Echo.Controllers
{
    [Route("")]
    public class IndexController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;
        private readonly ChatService _chatService;
        private readonly IStringLocalizer<IndexController> _localizer;

        public IndexController(AppDbContext context, UserService userService, ChatService chatService, IStringLocalizer<IndexController> localizer)
        {
            _context = context;
			_userService = userService;
			_chatService = chatService;
            _localizer = localizer;
        }

        [HttpGet]
        [Route("", Name = "Index")]
        public async Task<IActionResult> Index()
        {
			var userResult = await _userService.GetUserAsync(User);
			var usersResult = await _userService.GetUsersAsync(userResult.Result.Id);
			return View(usersResult.Result);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("403", Name = "403")]
        public IActionResult Page403()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("404", Name = "404")]
        public IActionResult Page404()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("error", Name = "Error")]
        public IActionResult ErrorPage()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("login", Name = "Login")]
        public IActionResult Login()
        {
			if (_userService.IsAuthenticated(User))
			{
				return RedirectToAction("Index");
			}
			else
			{
				var model = new LoginViewModel();
				return View(model);
			}
        }

        [HttpGet]
        [Route("logout", Name = "Logout")]
        public async Task<IActionResult> Logout()
        {
			await _userService.LogoutAsync();
			return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpPost]
		[ValidateAntiForgeryToken]
        [Route("signin", Name = "SignIn")]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
			if (_userService.IsAuthenticated(User))
			{
				return RedirectToAction("Index");
			}
			else
			{
				var result = await _userService.AuthenticateAsync(model.SignInUsername, model.SignInPassword);
				
				if (result.Result.Succeeded)
				{
					return RedirectToAction("index");
				}
				else
				{
					ModelState.AddModelError("SignInPassword", "Invalid username or password.");
					return View("Login");
				}
			}
        }

        [AllowAnonymous]
        [HttpPost]
		[ValidateAntiForgeryToken]
        [Route("signup", Name = "SignUp")]
        public async Task<IActionResult> SignUp(LoginViewModel model)
        {
			if (_userService.IsAuthenticated(User))
			{
				return RedirectToAction("index");
			}
			else
			{
				var result = await _userService.CreateUserAsync(model.Name, model.Username, model.Password);
				
				if (result.Succeeded)
				{
					return RedirectToAction("index");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("Password", error.Description);
					}
					return View("Login");
				}
			}
        }
    }
}
