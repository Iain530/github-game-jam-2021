// A GameState struct that contains a game ID, a list of players, and other game information.
pub struct GameState {
	pub game_id: String,
	
	pub bees: Vec<Bee>,
	pub tasks: Vec<Task>,
}

pub struct Bee {
	pub id: String,
	pub name: String,
	pub position: (f32, f32)
}

pub struct Task {
	pub id: String,
	pub complete: bool,
}