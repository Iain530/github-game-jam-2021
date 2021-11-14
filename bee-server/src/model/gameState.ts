import { v4 as uuidv4 } from 'uuid'
import { ClientsManager } from './clients'

export class GameStateManager {
	private static _instance: GameStateManager = new GameStateManager()
	private constructor() {
		if (GameStateManager._instance) {
			throw new Error('Error: Instantiation failed: Use GameStateManager.instance instead of new.')
		}
		GameStateManager._instance = this

		this.printGameStates()
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

		this.printGameStates()
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

	public startGame(gameId: any) {
		const gameState = this.getGameState(gameId)
		gameState.gameStarted = true
		// assign random queen
		const randomQueen = Math.floor(Math.random() * gameState.players.length)
		gameState.players[randomQueen].isQueenBee = true
		gameState.broadcastToPlayers()
	}

	public updatePlayerPosition(gameId: string, playerId: string, pos: [number, number]): boolean {
		const gameState = this.getGameState(gameId)
		const player = gameState.players.find(player => player.id === playerId)
		player.bee.position = pos
		gameState.broadcastToPlayers()
		return true
	}

	public completeTask(gameId: string, playerId: string, taskId: string): boolean {
		const gameState = this.getGameState(gameId)
		const player = gameState.players.find(player => player.id === playerId)
		if(gameState.tasks[player.currentTaskIndex].id == taskId) {
			gameState.tasks.find(task => task.id === taskId).complete = true
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
		this.printGameStates()
		return gameState
	}

	public updateAIBeePosition(gameId: string, beeId: string, pos: [number, number]): void {
		const gameState = this.getGameState(gameId)
		const bee = gameState.aiBees.find(bee => bee.id === beeId)
		bee.position = pos
		gameState.broadcastToPlayers()
	}

	public broadcastGameUpdates(gameId: string): void {
		const gameState = this.getGameState(gameId)
		gameState.broadcastToPlayers()
	}

	public deleteGameState(id: string): void {
		this.printGameStates()
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

	public assignTaskToPlayer(gameId: string, playerId: string, taskId: string): boolean {
		const gameState = this.getGameState(gameId)
		const player = gameState.players.find(player => player.id === playerId)
		player.currentTaskIndex = gameState.tasks.findIndex(task => task.id === taskId)
		gameState.broadcastToPlayers()
		return true
	}
}

export class GameState {
	gameId: string = uuidv4()
	messageTime: number = Math.round((new Date()).getTime() / 1000)
	gameCode: string = makeFourLetterID()
	gameStarted: boolean = false
	aiBees: Bee[] = []
	tasks: Task[] = []
	players: Player[]

	constructor(hostPlayer: Player) {
		this.players = [hostPlayer]

		const aiBeeCount = getRandomInt(8)
		for(let i = 0; i < aiBeeCount; i++) {
			const bee = new Bee(false)
			bee.name = 'AI Bee ' + (i + 1)
			this.aiBees.push(bee)
		}
	}

	broadcastToPlayers() {
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
	id: string = uuidv4()
	name: string = "Newbie"
	hatName: string = "Newbie"
	position: [number, number] = [0,0]
	isPlayer: boolean

	constructor(isPlayer: boolean) {
		this.isPlayer = isPlayer
	}
}

export class Player {
	id: string = uuidv4()
	bee: Bee = new Bee(true)
	currentTaskIndex: number = -1
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