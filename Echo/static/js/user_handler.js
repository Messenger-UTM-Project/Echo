export class User {
	static baseApiUrl = `${window.origin}/api/user`;

	static async getUuid() {
		const url = `${this.baseApiUrl}/getCurrentUuid`;

		try {
			const response = await fetch(url, {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				}
			});

			if (!response.ok) {
				throw new Error(`Error fetching UUID: ${response.statusText}`);
			}

			const data = await response.json();
			return data;
		} catch (error) {
			console.error('Error fetching current user UUID:', error);
			return null;
		}
    }

	static async getCurrentUser() {
		const url = `${this.baseApiUrl}/getCurrentUser`;

		try {
			const response = await fetch(url, {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				}
			});

			if (!response.ok) {
				throw new Error(`Error fetching User: ${response.statusText}`);
			}

			const data = await response.json();
			return data;
		} catch (error) {
			console.error('Error fetching current user User:', error);
			return null;
		}
    }
}
