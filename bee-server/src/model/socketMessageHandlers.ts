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
	const game = GameStateManager.instance.getGameState(data.gameId)
	if (client && game) {
		if(game.players[0].id === client.playerId) {
			GameStateManager.instance.startGame(game.gameId)
			ws.send(JSON.stringify({
				messageType: "START_GAME_RESPONSE",
				success: true,
			}))

		} else {
			ws.send(JSON.stringify({
				messageType: "START_GAME_RESPONSE",
				success: false,
				error: "You are not the host of this game"
			}))
		}
	}
}

export const playerPositionUpdateHandler = (ws: WebSocket, data: any) => {
	const client = ClientsManager.instance.getClient(data.secretToken)
	const success = GameStateManager.instance.updatePlayerPosition(data.gameId, client.playerId, data.position)
	ws.send(JSON.stringify({
		messageType: "PLAYER_POSITION_UPDATE_RESPONSE",
		success,
	}))
}

export const taskCompleteHandler = (ws: WebSocket, data: any) => {
	const client = ClientsManager.instance.getClient(data.secretToken)
	const success = GameStateManager.instance.completeTask(data.gameId, client.playerId, data.taskId)
	ws.send(JSON.stringify({
		messageType: "TASK_COMPLETE_RESPONSE",
		success,
	}))
}

export const aiPositionUpdateHandler = (ws: WebSocket, data: any) => {
	const client = ClientsManager.instance.getClient(data.secretToken)
	const game = GameStateManager.instance.getGameState(data.gameId)
	if (client && game) {
		if(game.players[0].id === client.playerId) {
			for(const pos of data.beePositions) {
				const {beeId, position: {x, y}} = pos
				GameStateManager.instance.updateAIBeePosition(data.gameId, beeId, [x, y])
				GameStateManager.instance.broadcastGameUpdates(data.gameId)
			}
			ws.send(JSON.stringify({
				messageType: "AI_POSITION_UPDATE_RESPONSE",
				success: true,
			}))
		} else {
			ws.send(JSON.stringify({
				messageType: "AI_POSITION_UPDATE_RESPONSE",
				success: false,
				error: "You are not the host of this game"
			}))
		}
	}
}