using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Echo.Data;
using Echo.Enum;
using Echo.Roles;
using Echo.Models;
using Echo.Repositories;
using Echo.Interfaces;

namespace Echo.Services
{
	public class FriendshipService
	{
		private readonly AppDbContext _context;
		private readonly UserRepository _userRepository;
		private readonly UserManager<User> _userManager;	
		private readonly AppRoleManager _roleManager;
		private readonly SignInManager<User> _signInManager;

		public FriendshipService(AppDbContext context, UserRepository userRepository, UserManager<User> userManager, AppRoleManager roleManager, SignInManager<User> signInManager)
		{
			_context = context;
			_userRepository = userRepository;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		public async Task<ServiceResult<Friendship>> CreateFriendshipAsync(Guid userId, Guid friendId)
		{
			if (userId == friendId)
				throw new ArgumentException("A user cannot be friends with themselves.");

			var existingFriendship = await _context.Friendships
				.FirstOrDefaultAsync(f => (f.User1Id == userId && f.User2Id == friendId) || (f.User1Id == friendId && f.User2Id == userId));

			var result = new ServiceResult<Friendship>(options =>
			{
				options.Result = null;
				options.StatusCode = HttpStatusCode.NotFound;
			});

			if (existingFriendship != null)
			{
				return result;
			}

			var friendship = new Friendship
			{
				User1Id = userId,
				User2Id = friendId,
				Status = FriendshipStatus.Pending
			};

			result = new ServiceResult<Friendship>(options =>
			{
				options.Result = friendship;
			});

			_context.Friendships.Add(friendship);
			await _context.SaveChangesAsync();

			return result;
		}

		public async Task<ServiceResult<Friendship>> AcceptFriendshipAsync(Guid userId, Guid friendId)
		{
			var friendship = await _context.Friendships
				.FirstOrDefaultAsync(f => f.User1Id == friendId && f.User2Id == userId && f.Status == FriendshipStatus.Pending);

			if (friendship == null)
				throw new InvalidOperationException("Friendship request not found.");

			friendship.Status = FriendshipStatus.Accepted;
			friendship.UpdatedAt = DateTime.UtcNow;

			var result = new ServiceResult<Friendship>(options =>
			{
				options.Result = friendship;
			});

			await _context.SaveChangesAsync();

			return result;
		}

		public async Task<ServiceResult<Friendship>> RejectFriendshipAsync(Guid userId, Guid friendId)
		{
			var friendship = await _context.Friendships
				.FirstOrDefaultAsync(f => f.User1Id == friendId && f.User2Id == userId && f.Status == FriendshipStatus.Pending);

			if (friendship == null)
				throw new InvalidOperationException("Friendship request not found.");

			friendship.Status = FriendshipStatus.Rejected;
			friendship.UpdatedAt = DateTime.UtcNow;

			var result = new ServiceResult<Friendship>(options =>
			{
				options.Result = friendship;
			});

			await _context.SaveChangesAsync();

			return result;
		}

		public async Task<ServiceResult<Friendship>> DeleteFriendshipAsync(Guid userId, Guid friendId)
		{
			var friendship = await _context.Friendships
				.FirstOrDefaultAsync(f => (f.User1Id == userId && f.User2Id == friendId) || (f.User1Id == friendId && f.User2Id == userId));

			var result = new ServiceResult<Friendship>(options =>
			{
				options.Result = null;
				options.StatusCode = HttpStatusCode.NotFound;
			});

			if (friendship == null)
			{
				return result;
			}

			result = new ServiceResult<Friendship>(options =>
			{
				options.Result = null;
			});

			_context.Friendships.Remove(friendship);
			await _context.SaveChangesAsync();

			return result;
		}
	}
}
