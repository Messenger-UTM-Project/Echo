using Microsoft.EntityFrameworkCore;

using Echo.Enum;
using Echo.Data;
using Echo.Models;

namespace Echo.Repositories
{
    public class UserRepository
	{
		private readonly AppDbContext _dbContext;

		public UserRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<User>> GetUsersAsync()
		{
			return await _dbContext.Users
				.Include(u => u.InitiatedFriendships)
				.Include(u => u.ReceivedFriendships)
				.ToListAsync();
		}

		public async Task<List<User>> GetUsersAsync(Guid id)
		{
			return await _dbContext.Users
				.Include(u => u.InitiatedFriendships)
				.Include(u => u.ReceivedFriendships)
				.Where(u => u.Id != id)
				.ToListAsync();
		}
		
		public async Task<User> FindByIdAsync(Guid id)
		{
			return await _dbContext.Users
				.FindAsync(id);
		}

		public async Task<User> FindByIdWithChatsAsync(Guid id)
		{
			return await _dbContext.Users
				.Include(u => u.OwnedChats)
				.Include(u => u.MemberChats)
				.ThenInclude(u => u.Messages)
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task<User> FindByIdWithFriendshipsAsync(Guid id)
		{
			return await _dbContext.Users
				.Include(u => u.InitiatedFriendships)
				.Include(u => u.ReceivedFriendships)
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task<User> FindByIdWithMessagesAsync(Guid id)
		{
			return await _dbContext.Users
				.Include(u => u.Messages)
				.AsSingleQuery()
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task<User> FindByIdWithAllAsync(Guid id)
		{
			return await _dbContext.Users
				.Include(u => u.OwnedChats)
				.Include(u => u.MemberChats)
				.ThenInclude(u => u.Messages)
				.Include(u => u.InitiatedFriendships)
				.Include(u => u.ReceivedFriendships)
				.Include(u => u.Messages)
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task<User> FindByUsernameAsync(string username)
		{
			return await _dbContext.Users
				.FirstOrDefaultAsync(u => u.UserName == username);
		}

		public async Task<User> FindByUsernameWithChatsAsync(string username)
		{
			return await _dbContext.Users
				.Include(u => u.OwnedChats)
				.Include(u => u.MemberChats)
				.ThenInclude(u => u.Messages)
				.FirstOrDefaultAsync(u => u.UserName == username);
		}

		public async Task<User> FindByUsernameWithFriendshipsAsync(string username)
		{
			return await _dbContext.Users
				.Include(u => u.InitiatedFriendships)
				.Include(u => u.ReceivedFriendships)
				.FirstOrDefaultAsync(u => u.UserName == username);
		}

		public async Task<User> FindByUsernameWithMessagesAsync(string username)
		{
			return await _dbContext.Users
				.Include(u => u.Messages)
				.AsSingleQuery()
				.FirstOrDefaultAsync(u => u.UserName == username);
		}

		public async Task<User> FindByUsernameWithAllAsync(string username)
		{
			return await _dbContext.Users
				.Include(u => u.OwnedChats)
				.Include(u => u.MemberChats)
				.ThenInclude(u => u.Messages)
				.Include(u => u.InitiatedFriendships)
				.Include(u => u.ReceivedFriendships)
				.Include(u => u.Messages)
				.FirstOrDefaultAsync(u => u.UserName == username);
		}

		public async Task<List<Chat>> GetOrderedChatsAsync(Guid userId)
		{
			return await _dbContext.Users
				.Where(u => u.Id == userId)
				.SelectMany(u => u.OwnedChats.Union(u.MemberChats))
				.Include(c => c.Messages)
				.ThenInclude(h => h.User)
				.OrderBy(c => c.Messages.Max(m => m.CreatedAt))
				.ToListAsync();
		}

		public async Task<List<User>> GetAllOutgoingFriendshipsAsync(Guid userId)
		{
			var initiatedFriends = await _dbContext.Friendships
				.Where(f => f.User1Id == userId && f.Status == FriendshipStatus.Pending)
				.Select(f => f.User2)
				.ToListAsync();

			return initiatedFriends;
		}

		public async Task<List<User>> GetAllIncomingFriendshipsAsync(Guid userId)
		{
			var receivedFriends = await _dbContext.Friendships
				.Where(f => f.User2Id == userId && f.Status == FriendshipStatus.Pending)
				.Select(f => f.User1)
				.ToListAsync();

			return receivedFriends;
		}

		public async Task<List<User>> GetAllFriendsAsync(Guid userId)
		{
			var initiatedFriends = await _dbContext.Friendships
				.Where(f => f.User1Id == userId && f.Status == FriendshipStatus.Accepted)
				.Select(f => f.User2)
				.ToListAsync();

			var receivedFriends = await _dbContext.Friendships
				.Where(f => f.User2Id == userId && f.Status == FriendshipStatus.Accepted)
				.Select(f => f.User1)
				.ToListAsync();

			return initiatedFriends.Concat(receivedFriends).Distinct().ToList();
		}
	}
}
