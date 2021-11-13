<script lang="ts">
	// const fetchGameState = 

import { beforeUpdate } from "svelte"

	let gameState = (async () => {
		const response = await fetch('http://192.168.0.48:8999/serverState?superSecretServerPassword=superSecretServerPassword')
    return await response.json()
	})()

	// setInterval(async () => {
	// 	gameState = await fetchGameState()
	// }, 10000)
</script>

<main>
	<h1>BEE Game Server Monitor</h1>
	{#await gameState}
		<p>...waiting</p>
	{:then data}
		<h2>Games:</h2>
		{#each data.gameState as game}
			<p>ID: {game.gameId}</p>
			<p>Code: {game.gameCode}</p>
			<p>Players: {game.players.length}</p>
			{#each game.players as player}
				<p>ID: {player.id}</p>
				<p>Bee {JSON.stringify(player.bee)}</p>
			{:else}
				<p>No players</p>
			{/each}
			<p>Tasks: {game.tasks.length}</p>
			{#each game.tasks as task}
				<p>ID: {task.id}</p>
				<p>Completed: {task.completed}</p>
			{:else}
			{/each}
			<hr />
		{:else}
			<p>No games</p>
		{/each}
	{:catch error}
		<p>An error occurred!</p>
	{/await}
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