import { v4 as uuidv4 } from 'uuid'
import { GameState } from './gameState'

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

	private clients: Map<string, Client> = new Map()
	private printClients(): void {
		console.log('Clients:')
		for (const client of this.clients.entries()) {
			console.log(client)
		}
	}

	public addClient(playerID: string, client: WebSocket): Client {
		const clientId = uuidv4() // this is also the secretToken
		const newClient = new Client(clientId, playerID, client)
		this.clients.set(clientId, newClient)

		// this.printClients()
		return newClient
	}

	public removeClient(clientId: string): void {
		const client = this.getClient(clientId)
		client.socket.close()
		this.clients.delete(clientId)
		// this.printClients()
	}

	public getClient(clientId: string): Client {
		if (this.clients.has(clientId)) {
			return this.clients.get(clientId)
		} else {
			console.error("Unknown client " + clientId)
			return null
		}
	}

	public getClientByPlayerID(playerID: string): Client {
		for (const client of this.clients.values()) {
			if (client.playerId === playerID) {
				return client
			}
		}
		return null
	}

	public updateGameStateForPlayer(playerId: string, gameState: GameState): void {
		const client = this.getClientByPlayerID(playerId)
		if (client) {
			client.socket.send(JSON.stringify({
				messageType: 'GAME_STATE_UPDATE',
				gameState
			}))
		}
	}
}

export class Client {
	public secretToken: string
	public playerId: string
	public socket: WebSocket

	constructor(secretToken: string, playerId: string, socket: WebSocket) {
		this.secretToken = secretToken
		this.playerId = playerId
		this.socket = socket
	}
}
