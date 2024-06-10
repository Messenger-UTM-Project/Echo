export class Friendship {
	static apiBaseUrl = `${window.origin}/api/friendship`;

	static async create(userId, friendId) {
		const url = `${this.apiBaseUrl}/create`;
		const body = JSON.stringify({ userId, friendId });

		const response = await fetch(url, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: body
		});

		if (!response.ok) {
			const error = await response.json();
			throw new Error(error.message);
		}

		return await response.json();
	}

	static async accept(userId, friendId) {
		const url = `${this.apiBaseUrl}/accept`;
		const body = JSON.stringify({ userId, friendId });

		const response = await fetch(url, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: body
		});

		if (!response.ok) {
			const error = await response.json();
			throw new Error(error.message);
		}

		return await response.json();
	}

	static async reject(userId, friendId) {
		const url = `${this.apiBaseUrl}/reject`;
		const body = JSON.stringify({ userId, friendId });

		const response = await fetch(url, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: body
		});

		if (!response.ok) {
			const error = await response.json();
			throw new Error(error.message);
		}

		return await response.json();
	}

	static async delete(userId, friendId) {
		const url = `${this.apiBaseUrl}/delete`;
		const body = JSON.stringify({ userId, friendId });

		const response = await fetch(url, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: body
		});

		if (!response.ok) {
			const error = await response.json();
			throw new Error(error.message);
		}

		return await response.json();
	}
}
