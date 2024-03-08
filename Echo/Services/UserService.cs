using Microsoft.AspNetCore.Identity;

using Echo.Enum;
using Echo.Models;
using Echo.Repositories;

namespace Echo.Services
{
    public class UserService
	{
		private readonly UserRepository _userRepository;
		private readonly UserManager<User> _userManager;	

		public UserService(UserRepository userRepository, UserManager<User> userManager)
		{
			_userRepository = userRepository;
			_userManager = userManager;
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

		public async Task<bool> CreateUserAsync(string username, string password)
		{
			var user = new User { UserName = username };
			var result = await _userManager.CreateAsync(user, password);

			return result.Succeeded;
		}
	}
}
