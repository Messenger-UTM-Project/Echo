export class Chat {
	static #connection = new signalR.HubConnectionBuilder()
			.withUrl("/chatHub")
			.configureLogging(signalR.LogLevel.Information)
			.build();

	static {
		Chat.#connection.start()
			.then(() => {
				console.log("Connection established.");
			})
			.catch(err => console.error(err));

		Chat.#connection.onclose(async () => {
			await Chat.#connection.start();
		});
		
		Chat.#connection.on("ReceiveMessage", (messageId, userId, chatId, message) => {
			console.log(`<${messageId}> [${chatId}] ${userId}: ${message}`);
		});
	}

	static async sendMessage(userId, chatId, message) {
		Chat.#connection.invoke("SendMessage", userId, chatId, message)
			.catch(err => console.error(err));
	}

	static async createChat(chatName, ownerUserIds, memberUserIds) {
		const url = `${window.location.origin}/api/chats/create`;

		const requestData = {
			ChatName: chatName,
			OwnerUserIds: ownerUserIds,
			MemberUserIds: memberUserIds
		};

		try {
			const response = await fetch(url, {method: 'POST', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(requestData)});

			if (!response.ok) {
				const errorDetails = await response.json();
				throw new Error(`Request failed: ${response.status} ${response.statusText} - ${errorDetails.message}`);
			}

			const result = await response.json();
			console.log('Chat created successfully:', result);
			return result;
		} catch (error) {
			console.error('Error creating chat:', error);
			throw error;
		}
	}
}
