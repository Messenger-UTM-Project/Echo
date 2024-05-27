using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using Echo.Models;
using Echo.Services;
using Echo.Interfaces;

namespace Echo.Hubs
{
    [Route("chatHub", Name = "ChatHub")]
    public class ChatHub : Hub
    {
		private readonly IUserConnectionManager _userConnectionManager;
		private readonly UserService _userService;
		private readonly ChatService _chatService;

		public ChatHub(IUserConnectionManager userConnectionManager, UserService userService, ChatService chatService)
		{
			_userConnectionManager = userConnectionManager;
			_userService = userService;
			_chatService = chatService;
		}

		public async Task SendMessage(Guid userUuid, Guid chatUuid, string messageContent)
		{
			var senderResult = await _userService.GetUserAsync(Context.User);
			var sender = senderResult.Result;
			
			if (sender == null)
				return;

			var result = await _chatService.GetChatByIdAsync(chatUuid);
			var chat = result.Result;
			if (chat == null)
				return;
			
			var isMember = await _chatService.IsUserInChatAsync(sender.Id, chatUuid);
			if (!isMember)
				return;

			var message = new Message
			{
				UserId = sender.Id,
				ChatId = chat.Id,
				Content = messageContent
			};

			await _chatService.CreateMessageAsync(message);

			var recipientConnectionId = _userConnectionManager.GetConnectionId(userUuid);

			if (recipientConnectionId != null)
				await Clients.Client(recipientConnectionId).SendAsync("ReceiveMessage", message.Id, sender.Id, chat.Id, messageContent);
		}

		public override async Task OnConnectedAsync()
		{
			var result = await _userService.GetUserAsync(Context.User);
			var user = result.Result;
			if (user != null)
			{
				_userConnectionManager.KeepUserConnection(user.Id, Context.ConnectionId);
			}

			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var result = await _userService.GetUserAsync(Context.User);
			var user = result.Result;
			if (user != null)
			{
				_userConnectionManager.RemoveUserConnection(user.Id, Context.ConnectionId);
			}

			await base.OnDisconnectedAsync(exception);
		}
    }
}
