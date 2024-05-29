using Microsoft.EntityFrameworkCore;

using Echo.Data;
using Echo.Models;

namespace Echo.Repositories
{
    public class ChatRepository
	{
		private readonly AppDbContext _dbContext;

		public ChatRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Chat> FindByIdAsync(Guid chatId)
		{
			return await _dbContext.Chats.FindAsync(chatId);
		}

		public async Task<Chat> GetChatWithMessagesAsync(Guid chatId)
		{
			var chat = await _dbContext.Chats
				.Include(c => c.Messages)
				.FirstOrDefaultAsync(c => c.Id == chatId);

			return chat;
		}
	}
}
