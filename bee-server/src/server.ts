import express from 'express';
import * as http from 'http';
import WebSocket, { Server } from "ws";
import { ClientsManager } from './model/clients';
import { GameState, GameStateManager, Player } from './model/gameState';
import { aiPositionUpdateHandler, assignTaskHandler, beeHatUpdateHandler, beeNameUpdateHandler, joinGameHandler, leaveLobbyHandler, playerPositionUpdateHandler, startGameHandler, taskCompleteHandler } from './model/socketMessageHandlers';

const app = express();

let allowCrossDomain = function(req, res, next) {
  res.header('Access-Control-Allow-Origin', "*");
  res.header('Access-Control-Allow-Headers', "*");
  next();
}
app.use(allowCrossDomain);

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

app.get('/serverState', (req, res) => {
	const superSecretServerPassword = req.query.superSecretServerPassword?.toString()!
	if(superSecretServerPassword !== 'superSecretServerPassword') {
		res.json({
			error: 'Invalid superSecretServerPassword'
		})
	} else {
		res.json({
			gameState: GameStateManager.instance.getAllGameStates()
		})
	}
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
				joinGameHandler(ws, data)
				break
			case 'START_GAME':
				startGameHandler(ws, data)
				break
			case 'PLAYER_POSITION_UPDATE':
				playerPositionUpdateHandler(ws, data)
				break
			case 'TASK_COMPLETE':
				taskCompleteHandler(ws, data)
				break
			case 'AI_POSITION_UPDATE':
				aiPositionUpdateHandler(ws, data)
				break
			case 'BEE_NAME_UPDATE':
				beeNameUpdateHandler(ws, data)
				break
			case 'BEE_HAT_UPDATE':
				beeHatUpdateHandler(ws, data)
				break
			case 'LEAVE_LOBBY':
				leaveLobbyHandler(ws, data)
				break
			case 'ASSIGN_TASK':
				assignTaskHandler(ws, data)
				break
			default:
				console.error(`Unknown message type: ${data.messageType}`)
				ws.send(JSON.stringify({
					success: false,
					error: `Unknown message type: ${data.messageType}`
				}))
				break
		}
	});
});

//start our server
server.listen(process.env.PORT || 8999, () => {
	console.log(`Server started on port ${(server.address() as WebSocket.AddressInfo).port} :)`);
});
