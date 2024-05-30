export class Chat {
	static apiBaseUrl = `${window.location.origin}/api/chats`;
	static connection = new signalR.HubConnectionBuilder()
			.withUrl("/chatHub")
			.configureLogging(signalR.LogLevel.Information)
			.build();

	static {
		this.connection.start()
			.then(() => {
				console.log("Connection established.");
			})
			.catch(err => console.error(err));

		this.connection.onclose(async () => {
			await this.connection.start();
		});
		
		this.connection.on("ReceiveMessage", (messageId, userId, chatId, message, createdAt, updatedAt, profileImagePath) => {
			console.log(`<${messageId}> [${chatId}] ${userId}: ${message}`);
		});
	}

	static async sendMessage(chatId, message) {
		this.connection.invoke("SendMessage", chatId, message)
			.catch(err => console.error(err));
	}

	static async createChat(chatName, ownerUserIds, memberUserIds) {
		const url = `${this.apiBaseUrl}/create`;

		const requestData = {
			ChatName: chatName,
			OwnerUserIds: ownerUserIds,
			MemberUserIds: memberUserIds
		};

		try {
			const response = await fetch(url, {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				},
				body: JSON.stringify(requestData)
			});

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
