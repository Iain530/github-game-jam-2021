pub struct Client {
  pub user_id: usize,
  pub game_state: GameState,
  
  pub sender: Option<mpsc::UnboundedSender<std::result::Result<Message, warp::Error>>>,
}