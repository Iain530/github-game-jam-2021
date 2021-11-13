import { ClientsManager } from "./clients"
import { GameStateManager } from "./gameState"

export const joinGameHandler = (ws: WebSocket, data: any) => {
	const client = ClientsManager.instance.addClient(data.playerId, ws)
	ws.send(JSON.stringify({
		success: true,
		gameState: GameStateManager.instance.getGameState(data.gameId),
		secretToken: client.secretToken
	}))
}

export const startGameHandler = (ws: WebSocket, data: any) => {
	const client = ClientsManager.instance.getClient(data.secretToken)
	GameStateManager.instance.startGame(data.gameId)
}