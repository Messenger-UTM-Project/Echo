using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

using Echo.Enum;
using Echo.Roles;
using Echo.Models;
using Echo.Repositories;
using Echo.Interfaces;

namespace Echo.Services
{
    public class UserService : IAuthService
	{
		private readonly UserRepository _userRepository;
		private readonly UserManager<User> _userManager;	
		private readonly AppRoleManager _roleManager;
		private readonly SignInManager<User> _signInManager;

		public UserService(UserRepository userRepository, UserManager<User> userManager, AppRoleManager roleManager, SignInManager<User> signInManager)
		{
			_userRepository = userRepository;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		public async Task<UserServiceResult<List<User>>> GetUsers()
		{
			var users = await _userRepository.GetUsersAsync();
			var result = new UserServiceResult<List<User>>();

			if (users.Count == 0)
				result.StatusCode = HttpStatusCode.NotFound;
			else
				result.Result = users;

			return result;
		}

		public async Task<IdentityResult> CreateUserAsync(string name, string username, string password)
		{
			var user = new User {
				UserName = username,
				Name = name
			};
			var result = await _userManager.CreateAsync(user, password);

			if (result.Succeeded)
			{
				await _roleManager.AssignRoleToUser(user, "User");
				var signInResult = await AuthenticateAsync(username, password);
			}

			return result;
		}

		public async Task<SignInResult> AuthenticateAsync(string username, string password)
		{
			var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
			return result;
		}

		public async Task LogoutAsync()
		{
			await _signInManager.SignOutAsync();
		}

		public bool IsAuthenticated(ClaimsPrincipal user)
		{
			return _signInManager.IsSignedIn(user);
		}
	}
}
