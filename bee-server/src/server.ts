import express from 'express';
import * as http from 'http';
import WebSocket, { Server } from "ws";
import { ClientsManager } from './model/clients';
import { GameState, GameStateManager, Player } from './model/gameState';

const app = express();

app.get('/createGame', (req, res) => {
	const hostPlayer = new Player()

	const gs = GameStateManager.instance.createGameState(hostPlayer)

	res.json({
		gameId: gs.gameId,
		gameCode: gs.gameCode,
		playerId: hostPlayer.id
	})
})

app.get('/joinGame', (req, res) => {
	const gameCode = req.query.gameCode?.toString()!
	const player = new Player()

	const gs = GameStateManager.instance.registerPlayerToGame(player, gameCode)

	res.json({
		gameId: gs.gameId,
		gameCode: gs.gameCode,
		playerId: player.id
	})
})

//initialize a simple http server
const server = http.createServer(app);

//initialize the WebSocket server instance
const wss = new Server({ server });

wss.on('connection', (ws: any) => { // This ws should have type WebSocket
	ws.on('message', (message: string) => {
		const data = JSON.parse(message)

		switch (data.messageType) {
			case 'JOIN_GAME_LOBBY':
				ClientsManager.instance.addClient(data.playerId, ws)
				ws.send(JSON.stringify({
					gameState: GameStateManager.instance.getGameState(data.gameId)
				}))
				break
			default:
				throw new Error(`Unknown message type: ${data.messageType}`)
		}
	});
});

//start our server
server.listen(process.env.PORT || 8999, () => {
	console.log(`Server started on port ${(server.address() as WebSocket.AddressInfo).port} :)`);
});
