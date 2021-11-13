export class ClientsManager {
	private static _instance: ClientsManager = new ClientsManager()
	private constructor() {
		if (ClientsManager._instance) {
			throw new Error('Error: Instantiation failed: Use ClientsManager.instance instead of new.')
		}
		ClientsManager._instance = this
	}
	public static get instance(): ClientsManager {
		return this._instance
	}

	private clients: Map<string, WebSocket> = new Map()
	private printClients(): void {
		console.log('Clients:')
		for (const client of this.clients.entries()) {
			console.log(client)
		}
	}

	public addClient(playerID: string, client: WebSocket): void {
		this.clients.set(playerID, client)

		this.printClients()
	}
}
