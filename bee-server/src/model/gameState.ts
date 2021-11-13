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

	startGame(gameId: any) {
		const gameState = this.getGameState(gameId)
		gameState.gameStarted = true
		gameState.broadcastToPlayers()
	}

	public registerPlayerToGame(player: Player, code: string): GameState {
		const gameState = this.getGameStateWithCode(code)
		gameState.players.push(player)
		gameState.bees.set(player.bee.id, player.bee)

		gameState.broadcastToPlayers()
		this.printGameStates()
		return gameState
	}

	public deleteGameState(id: string): void {
		this.printGameStates()
		this.gameStates.delete(id)
	}
}

export class GameState {
	gameId: string = uuidv4()
	messageTime: number = Math.round((new Date()).getTime() / 1000)
	gameCode: string = makeFourLetterID()
	gameStarted: boolean = false
	bees: Map<string, Bee> = new Map()
	tasks: Task[] = []
	players: Player[]

	constructor(hostPlayer: Player) {
		this.players = [hostPlayer]
		this.bees.set(hostPlayer.id, hostPlayer.bee)
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