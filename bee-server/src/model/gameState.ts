import { v4 as uuidv4 } from 'uuid'
import { ClientsManager } from './clients'

export class GameStateManager {
	private static _instance: GameStateManager = new GameStateManager()
	private constructor() {
		if (GameStateManager._instance) {
			throw new Error('Error: Instantiation failed: Use GameStateManager.instance instead of new.')
		}
		GameStateManager._instance = this

		// Garbage collection interval for inactive game states
		setInterval(() => {
			const currentTime = currentUnixTimestamp()
			for (const gameState of this.gameStates.values()) {
				const cleanupThreshold = gameState.gameStarted ? 30 : 60 * 2
				if(currentTime - gameState.lastUpdated > cleanupThreshold) {
					this.deleteGameState(gameState.gameId)
				}
			}
		}, 10000)

		// this.printGameStates()
	}
	public static get instance(): GameStateManager {
		return this._instance
	}

	private gameStates: Map<string, GameState> = new Map()

	private printGameStates(): void {
		console.log('GameStates:')
		for (const gameState of this.gameStates.values()) {
			console.log(gameState)
		}
	}

	public createGameState(hostPlayer: Player): GameState {
		const gameState = new GameState(hostPlayer)
		this.gameStates.set(gameState.gameId, gameState)

		// this.printGameStates()
		return gameState
	}

	public getGameState(id: string): GameState {
		return this.gameStates.get(id)!
	}

	public getAllGameStates(): GameState[] {
		return [...this.gameStates.values()]
	}

	public getGameStateWithCode(code: string): GameState {
		const res = [...this.gameStates.values()].filter(gameState => gameState.gameCode === code)
		if (res.length === 0) {
			return null
			console.error('No game state found with code: ' + code)
		} else {
			return res[0]
		}
	}

	public startGame(gameId: any, taskIds: string[]): void {
		const gameState = this.getGameState(gameId)
		gameState.gameStarted = true
		// assign AI bees
		const aiBeeCount = 10 + getRandomInt(5) - gameState.players.length
		for(let i = 0; i < aiBeeCount; i++) {
			const bee = new Bee(false)
			bee.name = 'AI Bee ' + (i + 1)
			gameState.aiBees.push(bee)
		}

		// assign random queen
		const randomQueen = Math.floor(Math.random() * gameState.players.length)
		gameState.players[randomQueen].isQueenBee = true
		gameState.players[randomQueen].bee.hatName = "Crown"

		const tasksPerPlayer = Math.floor(taskIds.length / gameState.players.length -1)
		const shuffledTasks = taskIds.map((value) => ({ value, sort: Math.random() }))
																.sort((a, b) => a.sort - b.sort)
																.map(({ value }) => value)
		// assign tasks to players
		for(let i = 0; i < gameState.players.length; i++) {
			const player = gameState.players[i]
			if(player.isQueenBee) continue
			for(let j = 0; j < tasksPerPlayer; j++) {
				player.tasks.push(new Task(shuffledTasks.pop()))
			}
		}

		gameState.broadcastToPlayers()
	}

	public updatePlayerPosition(gameId: string, playerId: string, pos: {x:number,y:number}): boolean {
		const gameState = this.getGameState(gameId)
		const player = gameState.players.find(player => player.id === playerId)
		player.bee.position = pos
		gameState.broadcastToPlayers()
		return true
	}

	public completeTask(gameId: string, playerId: string, taskId: string): boolean {
		const gameState = this.getGameState(gameId)
		const player = gameState.players.find(player => player.id === playerId)
		const task = player.tasks.find(task => task.id === taskId)
		if(task) {
			task.complete = true
			gameState.broadcastToPlayers()	
			return true
		} else {
			console.error('TaskId does not match')
			return false
		}
	}

	public registerPlayerToGame(player: Player, code: string): GameState {
		const gameState = this.getGameStateWithCode(code)
		if(gameState.gameStarted) {
			console.error('Game already started ' + code)
			return null
		}
		gameState.players.push(player)

		gameState.broadcastToPlayers()
		// this.printGameStates()
		return gameState
	}

	public updateAIBeePosition(gameId: string, beeId: string, pos:{x:number, y:number}): boolean {
		const gameState = this.getGameState(gameId)
		const bee = gameState.aiBees.find(bee => bee.id === beeId)
		if(bee) {
			bee.position = pos
			gameState.broadcastToPlayers()
			return true
		} 
		return false
	}

	public broadcastGameUpdates(gameId: string): void {
		const gameState = this.getGameState(gameId)
		gameState.broadcastToPlayers()
	}

	public deleteGameState(id: string): void {
		// this.printGameStates()
		this.getGameState(id).cleanGameSockets()
		this.gameStates.delete(id)
	}

	public updateBeeName(gameId: string, beeId: string, name: string, isPlayerBee: boolean): boolean {
		const gameState = this.getGameState(gameId)
		if(isPlayerBee) {
			const player = gameState.players.find(player => player.bee.id === beeId)
			player.bee.name = name
		} else {
			const bee = gameState.aiBees.find(bee => bee.id === beeId)
			bee.name = name
		}

		gameState.broadcastToPlayers()
		return true
	}

	public updateBeeHat(gameId: string, beeId: string, hat: string, isPlayerBee: boolean): boolean {
		const gameState = this.getGameState(gameId)
		if(isPlayerBee) {
			const player = gameState.players.find(player => player.bee.id === beeId)
			player.bee.hatName = hat
		} else {
			const bee = gameState.aiBees.find(bee => bee.id === beeId)
			bee.hatName = hat
		}

		gameState.broadcastToPlayers()
		return true
	}

	public unregisterPlayerFromGame(gameId: string, playerId: string): void {
		const gameState = this.getGameState(gameId)
		const player = gameState.players.find(player => player.id === playerId)
		gameState.players.splice(gameState.players.indexOf(player), 1)
		gameState.broadcastToPlayers()
	}

	public kickPlayerFromGame(gameId: string, playerId: string): boolean {
		const gameState = this.getGameState(gameId)
		const player = gameState.players.find(player => player.id === playerId)
		gameState.players.splice(gameState.players.indexOf(player), 1)
		gameState.broadcastToPlayers()
		return true
	}
	
	// Not used
	// public assignTaskToPlayer(gameId: string, playerId: string, taskId: string): boolean {
	// 	const gameState = this.getGameState(gameId)
	// 	const player = gameState.players.find(player => player.id === playerId)
	// 	player.currentTaskIndex = gameState.tasks.findIndex(task => task.id === taskId)
	// 	gameState.broadcastToPlayers()
	// 	return true
	// }

	public queenKill(gameId, queenPlayerId, targetBeeId): boolean {
		const gameState = this.getGameState(gameId)
		const queenPlayer = gameState.players.find(player => player.id === queenPlayerId)
		if(!queenPlayer.isQueenBee) {
			console.error('Player is not queen bee')
			return false
		}
		for(const player of gameState.players) {
			if(player.bee.id === targetBeeId) {
				player.bee.alive = false
				return true
			}
		}
		const targetBee = gameState.aiBees.find(bee => bee.id === targetBeeId)
		if(targetBee) {
			targetBee.alive = false
			gameState.broadcastToPlayers()
			return true
		}
		return false
	}
}

function currentUnixTimestamp(): number {
	return Math.floor(new Date().getTime() / 1000)
}

export class GameState {
	gameId: string = uuidv4()
	lastUpdated: number = currentUnixTimestamp()
	gameCode: string = makeFourLetterID()
	gameStarted: boolean = false
	aiBees: Bee[] = []
	players: Player[]

	constructor(hostPlayer: Player) {
		this.players = [hostPlayer]
	}

	cleanGameSockets() {
		this.players.forEach(player => {
			const client = ClientsManager.instance.getClientByPlayerID(player.id)
			if(client) client.socket.close
		})
	}

	broadcastToPlayers() {
		this.lastUpdated = currentUnixTimestamp()
		for(const player of this.players) {
			ClientsManager.instance.updateGameStateForPlayer(player.id, this)
		}
	}
}

export class Task {
	id: string
	complete: boolean = false

	constructor(id: string) {
		this.id = id
	}
}

export class Bee {
	static aiHatNames: string[] = ["Construction", "Fedora", "Popo", "Santa", "Suess", "Sorting",	"Tophat"]
	id: string = uuidv4()
	name: string = "Newbie"
	hatName: string = "Newbie"
	position: {x: number, y: number} = {x: 0, y: 0}
	isPlayer: boolean
	alive: boolean = true

	constructor(isPlayer: boolean) {
		this.isPlayer = isPlayer

		if(!isPlayer) {
			this.hatName = Bee.aiHatNames[getRandomInt(Bee.aiHatNames.length)]
		}
	}
}

export class Player {
	id: string = uuidv4()
	bee: Bee = new Bee(true)
	tasks: Task[] = []
	isQueenBee: boolean = false
}

function makeFourLetterID(): string {
  var text = ""
  const possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"

  for (var i = 0; i < 4; i++)
    text += possible.charAt(Math.floor(Math.random() * possible.length))

  return text
}

function getRandomInt(max) {
  return Math.floor(Math.random() * max);
}