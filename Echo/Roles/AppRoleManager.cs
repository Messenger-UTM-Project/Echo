using Microsoft.AspNetCore.Identity;

using Echo.Models;
using Echo.Roles;
using Echo.Repositories;

namespace Echo.Roles
{
	public class AppRoleManager
	{
		private readonly RoleManager<UserRole> _roleManager;
		private readonly UserManager<User> _userManager;
		private readonly UserRepository _userRepository;

		public AppRoleManager(RoleManager<UserRole> roleManager, UserManager<User> userManager, UserRepository userRepository)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_userRepository = userRepository;
		}

		public async Task CreateRoleAsync(string roleName)
		{
			if (!await _roleManager.RoleExistsAsync(roleName))
			{
				var role = new UserRole(roleName);
				await _roleManager.CreateAsync(role);
			}
		}

		public async Task AssignRoleToUserAsync(User user, string roleName)
		{
			if (user == null)
			{
				throw new ApplicationException($"User not found.");
			}

			var role = await _roleManager.FindByNameAsync(roleName);
			if (role == null)
			{
				throw new ApplicationException($"Role '{roleName}' not found.");
			}

			if (!await _userManager.IsInRoleAsync(user, roleName))
			{
				await _userManager.AddToRoleAsync(user, roleName);
			}
		}

		public async Task AssignRoleToUserAsync(Guid userId, string roleName)
		{
			var user = await _userRepository.FindByIdAsync(userId);
			await this.AssignRoleToUserAsync(user, roleName);
		}

		public async Task<bool> UserIsInRole(Guid userId, string roleName)
		{
			var user = await _userRepository.FindByIdAsync(userId);
			if (user == null)
			{
				throw new ApplicationException($"User with ID '{userId}' not found.");
			}

			return await _userManager.IsInRoleAsync(user, roleName);
		}
	}
}
