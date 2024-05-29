using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

using Echo.Data;
using Echo.Enum;
using Echo.Roles;
using Echo.Models;
using Echo.Repositories;
using Echo.Interfaces;

namespace Echo.Services
{
    public class UserService : IAuthService
	{
		private readonly AppDbContext _context;
		private readonly UserRepository _userRepository;
		private readonly UserManager<User> _userManager;	
		private readonly AppRoleManager _roleManager;
		private readonly SignInManager<User> _signInManager;

		public UserService(AppDbContext context, UserRepository userRepository, UserManager<User> userManager, AppRoleManager roleManager, SignInManager<User> signInManager)
		{
			_context = context;
			_userRepository = userRepository;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		public async Task<ServiceResult<List<User>>> GetUsersAsync()
		{
			var users = await _userRepository.GetUsersAsync();
			var result = new ServiceResult<List<User>>
			{
				Result = users,
				StatusCode = users.Count == 0 ? HttpStatusCode.NotFound : HttpStatusCode.OK
			};

			return result;
		}

		public async Task<ServiceResult<List<User>>> GetUsersAsync(Guid id)
		{
			var users = await _userRepository.GetUsersAsync(id);
			var result = new ServiceResult<List<User>>
			{
				Result = users,
				StatusCode = users.Count == 0 ? HttpStatusCode.NotFound : HttpStatusCode.OK
			};

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
				await _roleManager.AssignRoleToUserAsync(user, "User");
				var signInResult = await AuthenticateAsync(username, password);
			}

			return result;
		}

		public async Task<ServiceResult<SignInResult>> AuthenticateAsync(string username, string password)
		{
			var signInResult = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
			var result = new ServiceResult<SignInResult>
			{
				Result = signInResult,
				StatusCode = signInResult == null ? HttpStatusCode.Unauthorized : HttpStatusCode.OK
			};

			return result;
		}

		public async Task<bool> LogoutAsync()
		{
			try
			{
				await _signInManager.SignOutAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool IsAuthenticated(ClaimsPrincipal user)
		{
			return _signInManager.IsSignedIn(user);
		}

		public async Task<ServiceResult<User>> GetUserAsync(ClaimsPrincipal userPrincipal)
		{
			try
			{
				var user = await _userManager.GetUserAsync(userPrincipal);
				return CreateUserResult(user);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<ServiceResult<User>> GetUserAsync(Guid id)
		{
			try
			{
				var user = await _userRepository.FindByIdAsync(id);
				return CreateUserResult(user);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<ServiceResult<User>> GetUserAsync(string username)
		{
			try
			{
				var user = await _userRepository.FindByUsernameAsync(username);
				return CreateUserResult(user);
			}
			catch (Exception)
			{
				throw;
			}
		}

		private ServiceResult<User> CreateUserResult(User user)
		{
			var result = new ServiceResult<User>
			{
				Result = user,
				StatusCode = user == null ? HttpStatusCode.NotFound : HttpStatusCode.OK
			};

			return result;
		}

		public async Task<ServiceResult<List<User>>> GetAllFriendsAsync(ClaimsPrincipal userPrincipal)
		{
			var user = await _userManager.GetUserAsync(userPrincipal);
			var friends = await _userRepository.GetAllFriendsAsync(user.Id);
			var result = new ServiceResult<List<User>>
			{
				Result = friends,
				StatusCode = friends == null ? HttpStatusCode.NotFound : HttpStatusCode.OK
			};

			return result;
		}

		public async Task<ServiceResult<List<User>>> GetAllFriendsAsync(Guid userId)
		{
			var friends = await _userRepository.GetAllFriendsAsync(userId);
			var result = new ServiceResult<List<User>>
			{
				Result = friends,
				StatusCode = friends == null ? HttpStatusCode.NotFound : HttpStatusCode.OK
			};

			return result;
		}

		public async Task<ServiceResult<IdentityResult>> UpdateUserAsync(User user)
		{
			var updateResult = await _userManager.UpdateAsync(user);
			var result = new ServiceResult<IdentityResult>
			{
				Result = updateResult,
				StatusCode = updateResult.Succeeded ? HttpStatusCode.OK : HttpStatusCode.NotFound
			};

			return result;
		}
	}
}
