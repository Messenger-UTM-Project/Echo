using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

using Echo.Data;
using Echo.Models;
using Echo.Services;

namespace Echo.Controllers
{
    [Route("/profile")]
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<ProfileController> _localizer;
		private readonly IWebHostEnvironment _environment;
        private readonly UserService _userService;

        public ProfileController(AppDbContext context, IStringLocalizer<ProfileController> localizer, IWebHostEnvironment environment, UserService userService)
        {
            _context = context;
            _localizer = localizer;
			_environment = environment;
			_userService = userService;
        }

        [HttpGet]
        [Route("", Name = "Profile")]
        public async Task<IActionResult> Index()
        {
			var userResult = await _userService.GetUserAsync(User);
			var user = userResult.Result;
			ViewBag.Name = user.Name;
			ViewBag.UserName = user.UserName;
			ViewBag.ProfileImagePath = user.ProfileImagePath;
			var friendsResult = await _userService.GetAllFriendsAsync(user.Id);
            return View(friendsResult.Result);
        }

        [HttpGet]
        [Route("{guid:guid}", Name = "UUIDProfile")]
        public async Task<IActionResult> GetProfileByGuid(Guid guid)
        {
			var result = await _userService.GetUserAsync(guid);
			var user = result.Result;
			if (user == null)
				return NotFound("User not found.");
			ViewBag.Name = user.Name;
			ViewBag.UserName = user.UserName;
			ViewBag.ProfileImagePath = user.ProfileImagePath;
			var friendsResult = await _userService.GetAllFriendsAsync(user.Id);
            return View("Index", friendsResult.Result);
        }

        [HttpGet]
        [Route("{username}", Name = "UsernameProfile")]
        public async Task<IActionResult> GetProfileByUsername(string username)
        {
			var result = await _userService.GetUserAsync(username);
			var user = result.Result;
			if (user == null)
				return NotFound("User not found.");
			ViewBag.Name = user.Name;
			ViewBag.UserName = user.UserName;
			ViewBag.ProfileImagePath = user.ProfileImagePath;
			var friendsResult = await _userService.GetAllFriendsAsync(user.Id);
            return View("Index", friendsResult.Result);
        }

		[HttpPost]
        [Route("uploadProfileImage", Name = "UploadProfileImage")]
		public async Task<IActionResult> UploadProfileImage(IFormFile file)
		{
			if (file != null && file.Length > 0)
			{
				var result = await _userService.GetUserAsync(User);
				var user = result.Result;
				if (user != null)
				{
					var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

					var uploadsFolder = Path.Combine(_environment.WebRootPath, "media", "uploads", user.Id.ToString(), "profileImage");
					Directory.CreateDirectory(uploadsFolder);
					var filePath = Path.Combine(uploadsFolder, fileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}

					user.ProfileImagePath = $"/media/uploads/{user.Id}/profileImage/{fileName}";
					await _userService.UpdateUserAsync(user);
				}
			}

			return RedirectToAction("Profile");
		}
    }
}
