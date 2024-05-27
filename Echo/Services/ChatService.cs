using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Echo.Data;
using Echo.Enum;
using Echo.Roles;
using Echo.Models;
using Echo.Repositories;
using Echo.Interfaces;

namespace Echo.Services
{
    public class ChatService
	{
		private readonly AppDbContext _context;
		private readonly UserRepository _userRepository;
		private readonly ChatRepository _chatRepository;
		private readonly UserManager<User> _userManager;	
		private readonly AppRoleManager _roleManager;
		private readonly SignInManager<User> _signInManager;

		public ChatService(AppDbContext context, UserRepository userRepository, ChatRepository chatRepository, UserManager<User> userManager, AppRoleManager roleManager, SignInManager<User> signInManager)
		{
			_context = context;
			_userRepository = userRepository;
			_chatRepository = chatRepository;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		public async Task<ServiceResult<Chat>> CreateChatAsync(string chatName, List<Guid> ownerUserIds, List<Guid> memberUserIds)
		{
			var chat = new Chat
			{
				Name = chatName,
				Owners = new List<User>(),
				Members = new List<User>()
			};

			var result = new ServiceResult<Chat>(options => 
			{
				options.Result = chat;
			});

			foreach (var userId in ownerUserIds)
			{
				var user = await _context.Users.FindAsync(userId);
				if (user != null)
				{
					if (chat.Members.Any(m => m.Id == userId))
						throw new InvalidOperationException($"User with ID {userId} cannot be both an owner and a member.");

					chat.Owners.Add(user);
				}
				else
				{
					throw new Exception($"User with ID {userId} not found");
				}
			}

			foreach (var userId in memberUserIds)
			{
				var user = await _context.Users.FindAsync(userId);
				if (user != null)
				{
					if (chat.Owners.Any(o => o.Id == userId))
						throw new InvalidOperationException($"User with ID {userId} cannot be both an owner and a member.");

					chat.Members.Add(user);
				}
				else
				{
					throw new Exception($"User with ID {userId} not found");
				}
			}

			_context.Chats.Add(chat);
			await _context.SaveChangesAsync();

			return result;
		}

		public async Task<ServiceResult<Chat>> GetChatByIdAsync(Guid chatId)
		{
			var chat = await _chatRepository.FindByIdAsync(chatId);
			var result = new ServiceResult<Chat>(options => 
			{
				options.Result = chat;
			});
			
			if (chat == null)
				result.StatusCode = HttpStatusCode.NotFound;

			return result;
		}

		public async Task CreateMessageAsync(Message message)
		{
			_context.Messages.Add(message);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> IsUserMemberOfChatAsync(Guid userId, Guid chatId)
		{
			if (userId == Guid.Empty || chatId == Guid.Empty)
				return false;

			var user = await _userRepository.FindByIdWithChatsAsync(userId);
			if (user == null)
				return false;

			return user.MemberChats.Any(c => c.Id == chatId);
		}

		public async Task<bool> IsUserOwnerOfChatAsync(Guid userId, Guid chatId)
		{
			if (userId == Guid.Empty || chatId == Guid.Empty)
				return false;

			var user = await _userRepository.FindByIdWithChatsAsync(userId);
			if (user == null)
				return false;

			return user.OwnedChats.Any(c => c.Id == chatId);
		}

		public async Task<bool> IsUserInChatAsync(Guid userId, Guid chatId)
		{
			if (userId == Guid.Empty || chatId == Guid.Empty)
				return false;
			
			var user = await _userRepository.FindByIdWithChatsAsync(userId);

			if (user == null)
				return false;

			return user.OwnedChats.Any(c => c.Id == chatId) || user.MemberChats.Any(c => c.Id == chatId);
		}

		public async Task<ServiceResult<List<Chat>>> GetOrderedChatsAsync(ClaimsPrincipal userPrincipal)
		{
			var user = await _userManager.GetUserAsync(userPrincipal);
			var chats = await _userRepository.GetOrderedChatsAsync(user.Id);
			var result = new ServiceResult<List<Chat>>(options => 
			{
				options.Result = chats;
			});

			return result;
		}

		public async Task<ServiceResult<List<Chat>>> GetOrderedChatsAsync(Guid userId)
		{
			var chats = await _userRepository.GetOrderedChatsAsync(userId);
			var result = new ServiceResult<List<Chat>>(options => 
			{
				options.Result = chats;
			});

			return result;
		}
	}
}
