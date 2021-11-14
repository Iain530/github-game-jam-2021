<script lang="ts">
	const fetchGameState = async () => {
		const response = await fetch('http://192.168.0.48:8999/serverState?superSecretServerPassword=superSecretServerPassword')
    return await response.json()
	}

	let gameState = fetchGameState()

	setInterval(async () => {
		gameState = await fetchGameState()
	}, 5000)
</script>

<svelte:head>
<title>BEE Game Server Monitor</title>
<meta name="robots" content="noindex nofollow" />
<html lang="en" />
</svelte:head>

<main>
	<h1>BEE Game Server Monitor</h1>
	{#await gameState}
		<p>...waiting</p>
	{:then data}
		<h2>Games: {data.gameState.length}</h2>
		{#each data.gameState as game}
			<p>ID: {game.gameId} | Code: {game.gameCode} | Status: {game.gameStarted ? "Started" : "Not started"} </p>
			<p><b>Players</b>: {game.players.length}</p>
			{#each game.players as player}
				<p>ID: {player.id}</p>
				<p>Bee {JSON.stringify(player.bee)}</p>
			{:else}
				<p>No players</p>
			{/each}
			<p><b>Tasks</b>: {game.tasks.length}</p>
			{#each game.tasks as task}
				<p>ID: {task.id}</p>
				<p>Completed: {task.completed}</p>
			{/each}
			<p><b>AI bees</b>: {game.aiBees.length}</p>
			{#each game.aiBees as aiBee}
				<p>ID: {aiBee.id}</p>
				<p>Name: {aiBee.name}</p>
				<p>Position: {aiBee.position[0]}, {aiBee.position[1]}</p>
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