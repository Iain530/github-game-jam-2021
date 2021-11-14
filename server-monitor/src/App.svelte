<script lang="ts">
	const fetchGameState = async () => {
		const response = await fetch('http://192.168.0.48:8999/serverState?superSecretServerPassword=superSecretServerPassword')
    return await response.json()
	}

	let gameStateReq
	let gameState
	let loaded = false

	let currentTime = Math.floor(new Date().getTime() / 1000)
	
	setInterval(async () => {
		gameStateReq = await fetchGameState()
		currentTime = Math.floor(new Date().getTime() / 1000)
		gameState = gameStateReq.gameState
		gameState.forEach(game => {
			game.bees = [...game.players.map(player => player.bee), ...game.aiBees]
			game.secsSinceLastUpdate = currentTime - game.lastUpdated
		})
		loaded = true
	}, 1000)
</script>

<svelte:head>
<title>BEE Game Server Monitor</title>
<meta name="robots" content="noindex nofollow" />
<html lang="en" />
</svelte:head>

<main>
	<h1>BEE Game Server Monitor</h1>
	{#if !loaded}
		<p>...waiting</p>

		
	{:else}	
		<h2>Games: {gameState.length}</h2>
		{#each gameState as game}
			<!-- <div style="min-width: 1080px; min-height: 768px; background-color: #000;">
				{#each game.bees as bee}
					<div style="position: relative; top: {bee.y+500}px; left: {bee.x+500}px; width: 10px; height: 10px; background-color: {bee.isPlayer ? "yellow" : "blue"};"></div>
				{/each}
			</div> -->
			<h3>Game [last updated {game.secsSinceLastUpdate}s ago]</h3>
			<p>Game ID: {game.gameId} | Code: {game.gameCode} | Status: {game.gameStarted ? "Started" : "Not started"} </p>
			<p><b>Players</b>: {game.players.length}</p>
			{#each game.players as player}
				<p>Player ID: {player.id} {#if player.isQueenBee}| <b>QUEEN</b> {/if}</p>
				<p>Bee {JSON.stringify(player.bee)}</p>
				<p><b>Player tasks</b>: {player.tasks.length}</p>
				{#each player.tasks as task}
					<p>Task ID: {task.id} | Completed: {task.complete}</p>
				{/each}
			{:else}
				<p>No players</p>
			{/each}
			<p><b>AI bees</b>: {game.aiBees.length}</p>
			{#each game.aiBees as aiBee}
				<p>ID: {aiBee.id} | Name: {aiBee.name} | Hat: {aiBee.hatName} | Position: ({aiBee.position.x}, {aiBee.position.y})</p>
			{/each}
			<hr />
		{:else}
			<p>No games</p>
		{/each}
	{/if}
</main>

<style>
	main {
		text-align: center;
		padding: 1em;
		max-width: 240px;
		margin: 0 auto;
	}

	h1 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
	}

	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>