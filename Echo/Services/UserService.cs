using Microsoft.AspNetCore.Identity;

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

		public async Task<UserServiceResult> GetUsers()
		{
			var users = await _userRepository.GetUsersAsync();

			if (users.Count == 0)
			{
				return new UserServiceResult { StatusCode = 404, Users = null };
			}
			else
			{
				return new UserServiceResult { StatusCode = 200, Users = users };
			}
		}

		public async Task<bool> CreateUserAsync(string username, string password)
		{
			var user = new User { UserName = username };
			var result = await _userManager.CreateAsync(user, password);

			return result.Succeeded;
		}
	}
}
