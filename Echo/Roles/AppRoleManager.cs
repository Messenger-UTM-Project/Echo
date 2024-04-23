using Microsoft.AspNetCore.Identity;

using Echo.Models;
using Echo.Roles;

namespace Echo.Roles
{
	public class AppRoleManager
	{
		private readonly RoleManager<UserRole> _roleManager;
		private readonly UserManager<User> _userManager;

		public AppRoleManager(RoleManager<UserRole> roleManager, UserManager<User> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task CreateRole(string roleName)
		{
			if (!await _roleManager.RoleExistsAsync(roleName))
			{
				var role = new UserRole(roleName);
				await _roleManager.CreateAsync(role);
			}
		}

		public async Task AssignRoleToUser(User user, string roleName)
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

			await _userManager.AddToRoleAsync(user, roleName);
		}

		public async Task AssignRoleToUser(string userId, string roleName)
		{
			var user = await _userManager.FindByIdAsync(userId);
			await this.AssignRoleToUser(user, roleName);
		}

		public async Task<bool> UserIsInRole(string userId, string roleName)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new ApplicationException($"User with ID '{userId}' not found.");
			}

			return await _userManager.IsInRoleAsync(user, roleName);
		}
	}
}
