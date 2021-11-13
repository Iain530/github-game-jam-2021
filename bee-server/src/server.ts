import * as express from 'express';
import * as http from 'http';
import * as WebSocket from 'ws';
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
const wss = new WebSocket.Server({ server });

wss.on('connection', (ws: WebSocket) => {
	//connection is up, let's add a simple simple event
	ws.on('message', (message: string) => {
		//log the received message and send it back to the client
		console.log('received: %s', message);
		ws.send(`Hello, you sent -> ${message}`);
	});

	//send immediatly a feedback to the incoming connection    
	ws.send('Hi there, I am a WebSocket server');
});

//start our server
server.listen(process.env.PORT || 8999, () => {
	console.log(`Server started on port ${(server.address() as WebSocket.AddressInfo).port} :)`);
});
